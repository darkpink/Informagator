using Acadian.Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Acadian.Informagator.Threads
{
    public abstract class IntervalExecutionThread : Worker
    {
        protected virtual int MinSleepTime { get { return 500; } }
        protected virtual int MaxSleepTime { get { return 3000; } }
        protected virtual Func<int, int> GetSleepTime { get { return (t => Math.Min(t + MinSleepTime, MaxSleepTime)); } }

        public IntervalExecutionThread(ThreadConfiguration configuration)
            : base(configuration)
        {
        }

        public override void Run()
        {
            int currentSleepTime = 0;
            while(!StopRequested)
            {
                while (!(StopRequested || PauseRequested))
                if (Execute())
                {
                    currentSleepTime = 0;
                }
                else
                {
                    currentSleepTime = GetSleepTime(currentSleepTime);
                    Thread.Sleep(currentSleepTime);
                }
            }
        }

        protected abstract bool Execute();
        
    }
}
