using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.WorkerServices;
using Informagator.Contracts.Providers;
using Informagator.Contracts.Services;

namespace Informagator.Machine
{
    public class IsolatedWorkerHost : MarshalByRefObject
    {
        protected const int StopTimeoutMilliseconds = 10000;

        [ProvideToClient(typeof(IMessageStore))]
        protected IMessageStore MessageStore { get; set; }

        [ProvideToClient(typeof(IThreadConfiguration))]
        protected IThreadConfiguration Configuration { get; set; }

        protected Thread WorkerThread { get; set; }
        
        protected IWorker WorkerObject { get; set; }
        
        protected Type WorkerClass { get; set; }
        
        protected Exception WorkerThreadException { get; set; }

        [ProvideToClient(typeof(IAssemblyManager))]
        protected IAssemblyManager AssemblyManager { get; set; }

        protected UnityContainer Container { get; set; }

        public event Action<Exception> UnhandledException;

        protected ThreadRunStatus RunStatus { get; set; }

        public IsolatedWorkerHost(string machineName, string threadName)
        {
            try
            {
                RunStatus = ThreadRunStatus.NotStarted;
                Container = new UnityContainer();
                Container.LoadConfiguration();

                AssemblyManager = Container.Resolve<IAssemblyManager>();
                IConfigurationProvider configurationProvider = Container.Resolve<IConfigurationProvider>();
                Configuration = configurationProvider.GetMachineConfiguration(machineName).ThreadConfiguration[threadName];
                CreateWorker();
            }
            catch(Exception ex)
            {
                WorkerThreadException = ex;
                ConfigurationException newException = new ConfigurationException("Worker " + (threadName ?? "[Empty Name]") + " cannot be initialized.", ex);
                RunStatus = ThreadRunStatus.Error;
            }
        }

        public void Start()
        {
            if (RunStatus == ThreadRunStatus.NotStarted)
            {
                RunStatus = ThreadRunStatus.Running;
                WorkerThread = new Thread(new ThreadStart(RunWorker));
                WorkerThread.Start();
            }
        }

        protected void RunWorker()
        {
            try
            {
                WorkerObject.Start();
            }
            catch (Exception ex)
            {
                //configuration problems, who knows. No matter what the exception type is, the thread is done.
                RunStatus = ThreadRunStatus.Error;
                WorkerThreadException = ex;
                if (UnhandledException != null)
                {
                    UnhandledException(ex);
                }
            }
        }

        public void Stop()
        {
            if (RunStatus == ThreadRunStatus.Running)
            {
                WorkerObject.Stop();

                if (!WorkerThread.Join(StopTimeoutMilliseconds))
                {
                    WorkerThread.Abort();
                }
            }
        }

        public IThreadStatus Status
        {
            get
            {
                IThreadStatus result;

                result = new ThreadStatus(WorkerObject.Status);
                
                if (WorkerThreadException != null)
                {
                    result.Info = WorkerThreadException.ToString();
                }

                result.ThreadName = Configuration.Name;
                result.HostName = Environment.MachineName;
                result.RunStatus = RunStatus;
    
                return result;
            }
        }


        protected void CreateWorker()
        {
            //TODO: need good, descriptive configuration exceptions if any of these steps fail
            Assembly loadedAssembly = AssemblyManager.GetAssembly(Configuration.WorkerClassTypeAssembly);
            WorkerClass = loadedAssembly.GetType(Configuration.WorkerClassTypeName);
            WorkerObject = Activator.CreateInstance(WorkerClass) as IWorker;
            WorkerObject.Name = Configuration.Name;
            WorkerObject.Configuration = Configuration;

            var dependencyProps = WorkerClass.GetProperties().Where(pi => pi.GetCustomAttributes().OfType<HostProvidedAttribute>().Any());
            foreach (PropertyInfo pi in dependencyProps)
            {
                object value = GetWorkerDependencyValue(pi.PropertyType);
                pi.SetValue(WorkerObject, value);
            }

            //TODO: add validate settings on the worker.  this is the last point where I want configurationexceptions permitted
        }

        protected object GetWorkerDependencyValue(Type type)
        {
            object result;

            var myProperties = this.GetType().GetProperties(BindingFlags.Instance);
            var myPropertiesToProvideToClient = myProperties.Where(p => p.GetCustomAttributes().OfType<ProvideToClientAttribute>().Any());
            var myPropsWithCorrectType = myPropertiesToProvideToClient.FirstOrDefault(p => p.GetCustomAttributes().OfType<ProvideToClientAttribute>().Any(attr => attr.InterfaceType == type));
            
            if (myPropsWithCorrectType != null)
            {
                result = myPropsWithCorrectType.GetValue(this);
            }
            else
            {
                result = Container.Resolve(type);

                if (result == null)
                {
                    ConfigurationException ex = new ConfigurationException("ThreadHost to provide value for type " + type.ToString());
                    throw ex;
                }
            }
 	        
            return result;
        }
    }
}
