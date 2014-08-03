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
        protected IInformagatorWorker WorkerObject { get; set; }
        protected Type WorkerClass { get; set; }
        protected IThreadHostConfiguration WorkerConfiguration { get; set; }
        protected Dictionary<string, Assembly> LoadedAssemblies { get; set; }
        protected Exception WorkerThreadException { get; set; }
        public IAssemblySource AssemblySource { protected get; set; }
        public IMessageTracker MessageTracker { protected get; set; }
        public IMessageStore MessageStore { protected get; set; }
        public string Name { protected get; set; }
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
                    result.ThreadName = Name;
                }
                
                return WorkerObject.Status; 
            }
        }

        public IThreadHostConfiguration Configuration { get; set; }

        public void LoadAssembly(string name)
        {
            AssemblySource.LoadAssembly(name);
        }

        protected void CreateWorker()
        {
            Assembly loadedAssembly = AssemblySource.LoadAssembly(Configuration.WorkerClassTypeAssembly);
            WorkerClass = loadedAssembly.GetType(Configuration.WorkerClassTypeName);
            WorkerObject = Activator.CreateInstance(WorkerClass) as IInformagatorWorker;
            WorkerObject.Configuration = Configuration;
            foreach (string requiredAssembly in WorkerObject.RequiredAssemblies)
            {
                AssemblySource.LoadAssembly(requiredAssembly);
            }
        }


    }
}
