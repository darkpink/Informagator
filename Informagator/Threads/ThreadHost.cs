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

namespace Informagator.Threads
{
    internal class ThreadHost : MarshalByRefObject
    {
        protected const int StopTimeoutMilliseconds = 10000;

        protected Thread WorkerThread { get; set; }
        protected IInformagatorWorker WorkerObject { get; set; }
        protected Type WorkerClass { get; set; }
        protected ThreadConfiguration WorkerConfiguration { get; set; }
        protected Exception WorkerThreadException { get; set; }
        protected AssemblyManager AssemblyManager { get; set; }
        
        [ProvideToClient(typeof(IMessageStore))]
        [ProvideToClient(typeof(IMessageTracker))]
        [ProvideToClient(typeof(IConfigurationSource))]
        protected internal InternalServiceClient InternalService { get; set; }

        protected string Name { get; set; }

        public ThreadHost(string name)
        {
            InternalService = new InternalServiceClient();
            AssemblyManager = new AssemblyManager(InternalService);
            Name = name;
            Configuration = InternalService.GetThreadHostConfiguration(Name);
        }

        public void Start()
        {
            if (WorkerThread != null)
            {
                throw new InformagatorInvalidOperationException("Thread can only be started once.  Rebuild it.");
            }

            CreateWorker();
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

            foreach (string requiredAssembly in WorkerObject.RequiredAssemblies)
            {
                AssemblyManager.GetAssembly(requiredAssembly);
            }

            var dependencyProps = WorkerClass.GetProperties().Where(pi => pi.GetCustomAttributes().OfType<HostProvidedAttribute>().Any());
            var myPropsForClients = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Where(pi => pi.GetCustomAttributes().OfType<ProvideToClientAttribute>().Any());
            Dictionary<Type, object> availableDependencies = new Dictionary<Type, object>();
            foreach (PropertyInfo pi in myPropsForClients)
            {
                var attrs = pi.GetCustomAttributes<ProvideToClientAttribute>();
                var value = pi.GetValue(this);
                foreach(ProvideToClientAttribute attr in attrs)
                {
                    availableDependencies.Add(attr.InterfaceType, value);
                }
            }

            foreach (PropertyInfo pi in dependencyProps)
            {
                if (availableDependencies.ContainsKey(pi.PropertyType))
                {
                    pi.SetValue(WorkerObject, availableDependencies[pi.PropertyType]);
                }
            }

        }
    }
}
