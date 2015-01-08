using Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Informagator.Threads
{
    [Serializable]
    public abstract class IntervalExecutionThread : Worker
    {
        protected virtual int MinSleepTime { get { return 500; } }
        protected virtual int MaxSleepTime { get { return 3000; } }
        protected virtual Func<int, int> GetSleepTime { get { return (t => Math.Min(t + MinSleepTime, MaxSleepTime)); } }

        public override sealed void Run()
        {
            OnInitialize();

            int currentSleepTime = 0;
            while(!StopRequested)
            {
                HeartBeat = DateTime.Now;
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

        protected virtual void OnInitialize()
        {

        }
        protected abstract bool Execute();
        
    }
}
