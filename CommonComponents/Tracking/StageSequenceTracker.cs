using Informagator.Contracts;
using Informagator.Contracts.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.Tracking
{
    public class StageSequenceTracker
    {
        protected string MachineName { get; set; }

        protected string WorkerName { get; set; }

        protected Guid ProcessingSequenceId { get; set; }
        
        protected IMessageTracker MessageTracker { get; set;}

        protected int CurrentStageSequence { get; set; }
        
        protected string CurrentStageName { get; set; }

        protected int CurrentInputMessageSequence { get; set; }

        protected int CurrentOutputMessageSequence { get; set; }

        protected IMessage CurrentInputMessage { get; set; }

        protected Dictionary<IMessage, IMessageTrackingInfo> PreviousStageTrackingInfo { get; set; }
        
        protected Dictionary<IMessage, IMessageTrackingInfo> CurrentStageTrackingInfo { get; set; }
        
        public StageSequenceTracker(IMessageTracker messageTracker, string machineName, string workerName)
        {
            MessageTracker = messageTracker;
            MachineName = machineName;
            WorkerName = workerName;
        }

        public void TrackInitialSupplierStageOutput(IMessage initialMessage, string stage)
        {
            CurrentStageSequence = 0;
            ProcessingSequenceId = Guid.NewGuid();
            PreviousStageTrackingInfo = new Dictionary<IMessage, IMessageTrackingInfo>();
            CurrentStageTrackingInfo = new Dictionary<IMessage, IMessageTrackingInfo>();

            MessageTrackingInfo info = new MessageTrackingInfo(MachineName, WorkerName, ProcessingSequenceId, CurrentStageSequence, stage,
                null, 0);

            MessageTracker.TrackOutputMessage(info, initialMessage);
        }

        public void BeginNonInitialStage(string stage)
        {
            CurrentStageSequence++;
            CurrentStageName = stage;
            CurrentInputMessageSequence = -1;
        }

        public void BeginNonInitialStageInputMessage(IMessage message)
        {
            CurrentInputMessageSequence++;
            CurrentInputMessage = message;
            CurrentOutputMessageSequence = -1;
        }

        public void TrackNonInitialStageOutputMessage(IMessage message)
        {
            MessageTrackingInfo info = GetMessageTrackingInfo();
            MessageTracker.TrackOutputMessage(info, message);
        }

        public void TrackObserverStage(string stageName)
        {
            MessageTrackingInfo info = GetMessageTrackingInfo();
            info.Info = "Success";
            MessageTracker.TrackOutputMessage(info, CurrentInputMessage);
        }

        public void TrackReplyToInitialSupplierStage(string stageName, IMessage message)
        {
            MessageTrackingInfo info = GetMessageTrackingInfo();
            info.Info = "Reply to supplier";
            MessageTracker.TrackOutputMessage(info, message);
        }

        public void TrackStageException(Exception ex)
        {
            MessageTrackingInfo info = GetMessageTrackingInfo();
            info.Exception = ex.ToString();
            info.Info = ex.Message;
            MessageTracker.TrackOutputMessage(info, CurrentInputMessage); //TODO, is this a good idea?  Should pass null as message?
        }

        private MessageTrackingInfo GetMessageTrackingInfo()
        {
            CurrentOutputMessageSequence++;
            MessageTrackingInfo info = new MessageTrackingInfo(
                MachineName, WorkerName, ProcessingSequenceId, CurrentStageSequence, CurrentStageName, CurrentInputMessageSequence,
                CurrentOutputMessageSequence);

            return info;
        }
    }
}
