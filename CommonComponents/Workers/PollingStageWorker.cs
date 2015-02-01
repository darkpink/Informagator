using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Stages;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.WorkerServices;

namespace Informagator.CommonComponents.Workers
{
    public class PollingStageWorker : MessageWorker
    {
        [HostProvided]
        public IAssemblyManager AssemblyManager { get; set; }

        protected StageSequence Stages { get; set; }

        protected virtual int MinSleepTime { get { return 500; } }

        protected virtual int MaxSleepTime { get { return 3000; } }

        protected virtual Func<int, int> GetSleepTime { get { return (t => Math.Min(t + MinSleepTime, MaxSleepTime)); } }

        protected virtual bool Continue { get; set; }

        public override void Start()
        {
            base.Start();

            Continue = true;
            BuildStages();

            int currentSleepTime = 0;
            while (Continue)
            {
                HeartBeat = DateTime.Now;
                while (Continue)
                {
                    if (Stages.TryProcessMessage())
                    {
                        currentSleepTime = 0;
                        LastMessage = DateTime.Now;
                        Info = "Last message received at " + LastMessage.Value.ToString("MM/dd/yyyy HH:mm:ss");
                        MessageCount++;
                    }
                    else
                    {
                        currentSleepTime = GetSleepTime(currentSleepTime);
                        Thread.Sleep(currentSleepTime);
                    }
                }
            }
        }

        public override void Stop()
        {
            base.Stop();
            Continue = false;
        }

        protected virtual void BuildStages()
        {
            Stages = new StageSequence();
            Stages.MessageTracker = MessageTracker;
            
            foreach (IStageConfiguration stageConfig in Configuration.Stages)
            {
                IProcessingStage stage = AssemblyManager.CreateConfiguredObject(stageConfig, this) as IProcessingStage;
                var errorHandlers = new List<IMessageErrorHandler>();
                foreach(IErrorHandlerConfiguration errorHandlerConfiguration in stageConfig.ErrorHandlers)
                {
                    IMessageErrorHandler errorHandler = AssemblyManager.CreateConfiguredObject(errorHandlerConfiguration, stage) as IMessageErrorHandler;
                    errorHandlers.Add(errorHandler);
                }

                Stages.AddStage(stage, errorHandlers);
            }
        }
    }
}
