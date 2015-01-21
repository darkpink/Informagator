using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public enum ThreadRunStatus {  Unknown, NotStarted, Running, Stopped, Error };
    public interface IThreadStatus
    {
        string ThreadName { get; set; }
        
        string HostName { get; set; }

        ThreadRunStatus RunStatus { get; set; }
        
        DateTime? Initialized { get; set; }
        
        DateTime? RunningSince { get; set; }
        
        DateTime? Stopped { get; set; }
        
        DateTime? HeartBeat { get; set; }
        
        long MessageCount { get; set; }
        
        DateTime? LastMessage { get; set; }
        
        string Info { get; set; }
        
        bool IsRunning { get; set; }
    }
}
