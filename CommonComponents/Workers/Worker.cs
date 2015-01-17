using Informagator.CommonComponents;
using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.Workers
{
    public class Worker : IWorker
    {
        protected virtual int StopRequestTimeout { get { return 20000;}}
        protected Thread InnerThread { get; private set; }
        public virtual IThreadConfiguration Configuration { get; set; }
        protected virtual bool StopRequested { get; set; }
        protected virtual bool PauseRequested { get; set; }
        protected virtual bool IsRunning { get; set; }
        protected virtual DateTime? HeartBeat { get; set; }
        protected virtual DateTime? LastMessage { get; set; }
        protected virtual DateTime? Stopped { get; set; }
        protected virtual DateTime? Started { get; set; }
        protected virtual DateTime? Initialized { get; set; }
        protected virtual long MessageCount { get; set; }
        protected virtual string Info { get; set; }

        [HostProvided]
        [ProvideToClient(typeof(IMessageStore))]
        public virtual IMessageStore MessageStore { protected get; set; }
        public string Name { protected get; set; }

        [HostProvided]
        [ProvideToClient(typeof(IMessageTracker))]
        public virtual IMessageTracker MessageTracker { protected get; set; }

        public Worker()
        {
            HeartBeat = DateTime.Now;
            Initialized = DateTime.Now;
            Info = "Initialized";
        }

        public virtual void Start()
        {
            if (!IsRunning)
            {
                Started = DateTime.Now;
                Info = "Started";
                InnerThread = new Thread(new ThreadStart(ThreadEntry));
                InnerThread.Start();
                IsRunning = true;
            }
        }

        private void ThreadEntry()
        {
            try
            {
                Run();
            }
            catch (ThreadAbortException)
            {
                Info = "Aborted";
                throw;
            }
            catch (Exception ex)
            {
                Info = ex.Message;
                throw;
            }
        }

        public virtual void Run()
        {
        }

        public virtual void Stop()
        {
            if (IsRunning)
            {
                StopRequested = true;
                Info = "Stop Requested";
                bool successfullJoin = InnerThread.Join(StopRequestTimeout);
                if (successfullJoin)
                {
                    Info = "Stopped";
                }
                else
                {
                    Info = String.Format("Stop requested but unresponsive after {0} seconds - Aborting.", (StopRequestTimeout / 1000));
                    InnerThread.Abort();
                }

                IsRunning = false;
                Stopped = DateTime.Now;
            }
        }

        public virtual IThreadStatus Status
        {
            get 
            {
                WorkerThreadStatus result = new WorkerThreadStatus();
                result.IsRunning = IsRunning;
                result.HostName = Dns.GetHostName();
                result.HeartBeat = HeartBeat;
                result.Initialized = Initialized;
                result.LastMessage = LastMessage;
                result.MessageCount = MessageCount;
                result.RunningSince = Started;
                result.Stopped = Stopped;
                result.Info = Info;

                if (Configuration != null)
                {
                    result.ThreadName = Configuration.Name;
                }
                else
                {
                    result.ThreadName = "Not Configured";
                }

                return result;
            }
        }



        public IList<string> RequiredAssemblies
        {
            get
            { 
                return Configuration.StageConfigurations.Select(sc => sc.StageAssemblyName)
                         .Union(Configuration.StageConfigurations.Select(sc => sc.ErrorHandlerAssemblyName))
                         .ToList();
            }
        }


        public virtual void ValidateSettings()
        {
        }
    }
}
