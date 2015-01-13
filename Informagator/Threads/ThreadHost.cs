using Informagator.Configuration;
using Informagator.Exceptions;
using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Informagator.Services;
using System.ServiceModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Informagator.Threads
{
    internal class ThreadHost : MarshalByRefObject
    {
        protected const int StopTimeoutMilliseconds = 10000;

        public IMessageStore MessageStore { get; set; }
        
        protected Thread WorkerThread { get; set; }
        
        protected IInformagatorWorker WorkerObject { get; set; }
        
        protected Type WorkerClass { get; set; }
        
        protected ThreadConfiguration WorkerConfiguration { get; set; }
        
        protected Exception WorkerThreadException { get; set; }

        protected AssemblyManager AssemblyManager { get; set; }

        protected UnityContainer Container { get; set; }

        protected string Name { get; set; }

        public Type[] PersistentServices { get; }

        public ThreadHost(string name)
        {
            Container = new UnityContainer();
            Container.LoadConfiguration();

            //TODO: throw a good exception if either of these resolutions fail
            AssemblyManager = new AssemblyManager(Container.Resolve<IAssemblySource>());
            IConfigurationProvider configurationProvider = Container.Resolve<IConfigurationProvider>();
            Container.RegisterInstance<AssemblyManager>(AssemblyManager);
            Name = name;
            Configuration = configurationProvider.Configuration.ThreadConfiguration[Name];
            CreateWorker();
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
            catch (ThreadAbortException)
            { 
                //normal-ish.
            }
            catch (Exception ex)
            {
                WorkerThreadException = ex;
                //throw?  Need to figure out what to do here.
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

        public InformagatorThreadStatus Status
        {
            get
            {
                InformagatorThreadStatus result;

                if (WorkerObject != null)
                {
                    result = WorkerObject.Status;
                }
                else
                {
                    result = new InformagatorThreadStatus();
                    result.HostName = Environment.MachineName;
                    result.Info = "Not Started";
                    result.ThreadName = Name;
                }
                
                return WorkerObject.Status; 
            }
        }


        protected ThreadConfiguration Configuration { get; set; }

        public void LoadAssembly(string name)
        {
            AssemblyManager.GetAssembly(name);
        }

        protected void CreateWorker()
        {
            Assembly loadedAssembly = AssemblyManager.GetAssembly(Configuration.WorkerClassTypeAssembly);
            WorkerClass = loadedAssembly.GetType(Configuration.WorkerClassTypeName);
            WorkerObject = Activator.CreateInstance(WorkerClass) as IInformagatorWorker;
            WorkerObject.Name = Name;
            WorkerObject.Configuration = Configuration;

            var dependencyProps = WorkerClass.GetProperties().Where(pi => pi.GetCustomAttributes().OfType<HostProvidedAttribute>().Any());
            foreach (PropertyInfo pi in dependencyProps)
            {
                //get it from the IoC container for now
                //TODO: throw a good exception if not found
                object value = Container.Resolve(pi.PropertyType);
                pi.SetValue(WorkerObject, value);
            }
        }
    }
}
