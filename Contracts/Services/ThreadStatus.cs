using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Services
{
    [DataContract]
    [Serializable]
    public class ThreadStatus : IThreadStatus
    {
        [DataMember]
        public string ThreadName {get;set;}

        [DataMember]
        public string HostName {get;set;}

        [DataMember]
        public ThreadRunStatus RunStatus { get;set;}

        [DataMember]
        public DateTime? Initialized {get;set;}

        [DataMember]
        public DateTime? RunningSince {get;set;}

        [DataMember]
        public DateTime? Stopped {get;set;}

        [DataMember]
        public DateTime? HeartBeat { get;set;}

        [DataMember]
        public long MessageCount { get; set;  }

        [DataMember]
        public DateTime? LastMessage { get;set;}

        [DataMember]
        public string Info { get; set; }

        [DataMember]
        public bool IsRunning { get; set; }

        public ThreadStatus(IThreadStatus status)
        {
            IsRunning = status.IsRunning;
            Info = status.Info;
            LastMessage = status.LastMessage;
            MessageCount = status.MessageCount;
            HeartBeat = status.HeartBeat;
            Stopped = status.Stopped;
            RunningSince = status.RunningSince;
            Initialized = status.Initialized;
            RunStatus = status.RunStatus;
            HostName = status.HostName;
            ThreadName = status.ThreadName;
        }
    }
}
