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

        [ProvideToClient(typeof(IMessageTracker))]
        protected IMessageTracker MessageTracker { get; set; }

        [ProvideToClient(typeof(IWorkerConfiguration))]
        protected IWorkerConfiguration Configuration { get; set; }

        protected Thread WorkerThread { get; set; }
        
        protected IWorker WorkerObject { get; set; }
        
        //protected Type WorkerClass { get; set; }
        
        protected Exception WorkerThreadException { get; set; }

        [ProvideToClient(typeof(IAssemblyManager))]
        protected IAssemblyManager AssemblyManager { get; set; }

        protected UnityContainer Container { get; set; }

        public event Action<Exception> UnhandledException;

        protected ThreadRunStatus RunStatus { get; set; }

        protected List<IMessageErrorHandler> ErrorHandlers { get; set; }

        public IsolatedWorkerHost(string machineName, string threadName)
        {
            try
            {
                RunStatus = ThreadRunStatus.NotStarted;
                ErrorHandlers = new List<IMessageErrorHandler>();
                Container = new UnityContainer();
                Container.LoadConfiguration();

                AssemblyManager = Container.Resolve<IAssemblyManager>();
                MessageStore = Container.Resolve<IMessageStore>();
                MessageTracker = Container.Resolve<IMessageTracker>();
                IConfigurationProvider configurationProvider = Container.Resolve<IConfigurationProvider>();
                Configuration = configurationProvider.GetMachineConfiguration(machineName).Workers[threadName];
                CreateErrorHandlers();
                CreateWorker();
            }
            catch(Exception ex)
            {
                WorkerThreadException = ex;
                ConfigurationException newException = new ConfigurationException("Worker " + (threadName ?? "[Empty Name]") + " cannot be initialized.", ex);
                RunStatus = ThreadRunStatus.Error;
            }
        }

        private void CreateErrorHandlers()
        {
            foreach (IErrorHandlerConfiguration errorHandlerConfiguration in Configuration.ErrorHandlers)
            {
                IMessageErrorHandler errorHandler = AssemblyManager.CreateConfiguredObject(errorHandlerConfiguration, this) as IMessageErrorHandler;
                ErrorHandlers.Add(errorHandler);
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
                InvokeErrorHandlers(ex);
                if (UnhandledException != null)
                {
                    UnhandledException(ex);
                }
            }
        }

        private void InvokeErrorHandlers(Exception ex)
        {
            foreach(IMessageErrorHandler errorHandler in ErrorHandlers)
            {
                errorHandler.Handle(new[] { "Unhandled exception on thread " + Configuration.Name }, ex, null);
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
            WorkerObject = AssemblyManager.CreateConfiguredObject(Configuration, this) as IWorker;
            WorkerObject.Configuration = Configuration;
            WorkerObject.ValidateSettings();
        }


        public bool AnyAssemblyChanged
        {
            get
            {
                return AssemblyManager.AnyAssemblyChanged;
            }
        }

        public virtual bool IsRestartRequiredForNewConfiguration(IWorkerConfiguration newConfiguration)
        {
            bool result;

            if (RunStatus == ThreadRunStatus.Running)
            {
                return WorkerObject.IsRestartRequiredForNewConfiguration(newConfiguration);
            }
            else
            {
                result = true;
            }
            
            return result;
        }
    }
}
