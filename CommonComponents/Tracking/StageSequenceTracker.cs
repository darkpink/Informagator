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
        protected Guid ProcessingSequenceId { get; set; }
        protected IMessageTracker MessageTracker { get; set;}

        protected int CurrentStageSequence { get; set; }
        protected string CurrentStageName { get; set; }

        protected int CurrentMessageSequence { get; set; }

        protected IMessage CurrentInputMessage { get; set; }

        protected Dictionary<IMessage, IMessageTrackingInfo> PreviousStageTrackingInfo { get; set; }
        protected Dictionary<IMessage, IMessageTrackingInfo> CurrentStageTrackingInfo { get; set; }
        public StageSequenceTracker(IMessageTracker messageTracker)
        {
            MessageTracker = messageTracker;
            ProcessingSequenceId = Guid.NewGuid();
            PreviousStageTrackingInfo = new Dictionary<IMessage, IMessageTrackingInfo>();
            CurrentStageTrackingInfo = new Dictionary<IMessage, IMessageTrackingInfo>();
            CurrentMessageSequence = 0;
        }

        public void TrackInitialSupplierStageOutput(IMessage initialMessage, string stage)
        {
            CurrentMessageSequence++;
            CurrentStageSequence = 0;
        }

        public void TrackNoninitialStageOutputMessage(IMessage message)
        {
        }

        public void BeginNonInitialStage(string stage)
        {
        }

        public void BeginNonInitialStageInputMessage(IMessage message)
        {
        }

        public void TrackNonInitialStageOutputMessage(IMessage message)
        {
        }

        public void TrackObserverStage(string stageName)
        {

        }

        public void TrackReplyToInitialSupplierStage(string stageName, IMessage message)
        {

        }

        public void TrackStageException(string stageName, IMessage message, Exception ex)
        {

        }
    }
}
