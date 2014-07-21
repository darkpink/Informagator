using Acadian.Informagator.Configuration;
using Acadian.Informagator.Exceptions;
using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Acadian.Informagator.Threads
{
    public class ThreadHost : MarshalByRefObject, IInformagatorThreadHost
    {
        protected const int StopTimeoutMilliseconds = 10000;

        protected Thread WorkerThread { get; set; }
        protected IInformagatorWorkerClass WorkerObject { get; set; }
        protected Type WorkerClass { get; set; }
        protected IThreadConfiguration WorkerConfiguration { get; set; }
        protected Dictionary<string, Assembly> LoadedAssemblies { get; set; }
        protected Exception WorkerThreadException { get; set; }
        protected IAssemblySource AssemblyLoader { get; set; }
        public ThreadHost()
        {
            LoadedAssemblies = new Dictionary<string, Assembly>();
        }

        public void Start()
        {
            if (WorkerThread != null)
            {
                throw new InformagatorInvalidOperationException("Thread can only be started once.  Rebuild it.");
            }

            if (WorkerObject == null)
            {
                throw new InformagatorInvalidOperationException("Worker Object must be created before starting");
            }

            WorkerThread = new Thread(new ThreadStart(RunWorker));
            WorkerThread.Start();
        }

        protected void RunWorker()
        {
            try
            {
                WorkerObject.Run();
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

        public void Pause()
        {
            WorkerObject.Pause();
        }

        public void Resume()
        {
            WorkerObject.Resume();
        }

        public void Stop()
        {
            WorkerObject.Stop();
            
            if (!WorkerThread.Join(StopTimeoutMilliseconds))
            {
                WorkerThread.Abort();
            }
        }

        public IInformagatorThreadStatus Status
        {
            get
            {
                IInformagatorThreadStatus result;

                if (WorkerObject != null)
                {
                    result = WorkerObject.Status;
                }
                else
                {
                    result = new InformagatorThreadStatus();
                    result.HostName = Dns.GetHostName();
                    result.Info = "Not Started";
                    result.ThreadName = WorkerConfiguration == null ? "Configuration Not Yet Loaded" : WorkerConfiguration.Name;
                }
                
                return WorkerObject.Status; 
            }
        }

        public IThreadConfiguration Configuration
        {
            set 
            {
                WorkerConfiguration = value;
            }
        }

        public IAssemblySource AssemblySource
        {
            set
            {
                AssemblyLoader = value;
            }
        }
        public void LoadAssembly(string name)
        {
            AssemblyLoader.LoadAssembly(name);
        }

        public void CreateWorker(string assemblyPath, string type)
        {
            Assembly loadedAssembly = AssemblyLoader.LoadAssembly(assemblyPath);
            WorkerClass = loadedAssembly.GetType(type);
            ConstructorInfo constructor = WorkerClass.GetConstructor(new[] { typeof(IThreadConfiguration) });
            WorkerObject = constructor.Invoke(new object[] {WorkerConfiguration}) as IInformagatorWorkerClass;
        }
    }
}
