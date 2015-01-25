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
        public virtual IThreadConfiguration Configuration { get; set; }

        protected ThreadRunStatus RunStatus { get; set; }
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
            RunStatus = ThreadRunStatus.NotStarted;
        }

        public virtual void Start()
        {
            RunStatus = ThreadRunStatus.Running;
            Started = DateTime.Now;
            Info = "Started";
            Run();
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
            StopRequested();
            Info = "Stop Requested";
        }

        protected virtual void StopRequested()
        {
        }

        public virtual IThreadStatus Status
        {
            get 
            {
                WorkerThreadStatus result = new WorkerThreadStatus();
                result.RunStatus = RunStatus;
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

        public virtual void ValidateSettings()
        {
        }


        public virtual bool IsRestartRequiredForNewConfiguration(IThreadConfiguration newConfiguration)
        {
            return true;
        }
    }
}
