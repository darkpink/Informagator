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
using Informagator.Contracts.PersistentServices;
using Informagator.Contracts.WorkerServices;
using Informagator.Contracts.Providers;

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

        public IPersistentServiceSignature[] RequiredPersistentServices 
        { 
            get
            {
                IPersistentServiceSignature[] result;

                if (WorkerObject is IPersistentServiceClient)
                {
                    result = ((IPersistentServiceClient)WorkerObject).RequiredPersistentServices;
                }
                else
                {
                    result = new IPersistentServiceSignature[0];
                }

                return result;
            }
        }

        public IsolatedWorkerHost(string name)
        {
            try
            {
                Container = new UnityContainer();
                Container.LoadConfiguration();

                AssemblyManager = Container.Resolve<IAssemblyManager>();
                IConfigurationProvider configurationProvider = Container.Resolve<IConfigurationProvider>();
                Configuration = configurationProvider.Configuration.ThreadConfiguration[name];
                CreateWorker();
            }
            catch(Exception ex)
            {
                WorkerThreadException = ex;
                ConfigurationException newException = new ConfigurationException("Worker " + (name ?? "[Empty Name]") + " cannot be initialized.", ex);
                throw newException;
            }
        }

        public void Start()
        {
            if (WorkerThread != null)
            {
                throw new InformagatorInvalidOperationException("Thread can only be started once.  Rebuild it.");
            }

            WorkerThread = new Thread(new ThreadStart(RunWorker));
            
            WorkerThread.Start();
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
                WorkerThreadException = ex;
                UnhandledException(ex);
            }
        }

        public void Stop()
        {
            WorkerObject.Stop();
            
            if (!WorkerThread.Join(StopTimeoutMilliseconds))
            {
                WorkerThread.Abort();
            }
        }

        public IThreadStatus Status
        {
            get
            {
                IThreadStatus result;

                result = WorkerObject.Status;
                
                if (WorkerThreadException != null)
                {
                    result.RunStatus = ThreadRunStatus.Error;
                    result.Info = WorkerThreadException.ToString();
                }

                result.ThreadName = Configuration.Name;
                result.HostName = Environment.MachineName;
                    
                return result;
            }
        }


        protected void CreateWorker()
        {
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
