using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Tracking
{
    public class ProcessingSequenceTracker
    {
        protected Guid ProcessingSequenceId { get; set; }
        protected IMessageTracker MessageTracker { get; set;}

        protected int CurrentStageSequence { get; set; }
        protected string CurrentStageName { get; set; }

        protected int CurrentMessageSequence { get; set; }

        protected IMessage CurrentInputMessage { get; set; }

        protected Dictionary<IMessage, TrackingInfo> PreviousStageTrackingInfo { get; set; }
        protected Dictionary<IMessage, TrackingInfo> CurrentStageTrackingInfo { get; set; }
        public ProcessingSequenceTracker(Guid processingSequenceId, IMessageTracker messageTracker)
        {
            MessageTracker = messageTracker;
            ProcessingSequenceId = processingSequenceId;
            PreviousStageTrackingInfo = new Dictionary<IMessage, TrackingInfo>();
            CurrentStageTrackingInfo = new Dictionary<IMessage, TrackingInfo>();
            CurrentStageSequence = 0;
            CurrentMessageSequence = 0;
        }

        public void TrackInitialMessage(IMessage initialMessage, string stage)
        {
            TrackingInfo initialTrackingInfo = new TrackingInfo(ProcessingSequenceId, 0, stage, 0);
            MessageTracker.TrackMessage(initialTrackingInfo, initialMessage);
            CurrentStageTrackingInfo.Add(initialMessage, initialTrackingInfo);
            CurrentStageName = stage;
        }

        public void TrackSequenceOutputMessage(IMessage message)
        {
            TrackingInfo previousTrackingInfo = PreviousStageTrackingInfo[CurrentInputMessage];
            TrackingInfo newTrackingInfo = previousTrackingInfo.GetNextInSequence(CurrentStageName, CurrentMessageSequence);
            MessageTracker.TrackMessage(newTrackingInfo, message);
            CurrentMessageSequence++;
        }

        public void BeginStage(string stage)
        {
            CurrentStageSequence++;
            CurrentStageName = stage;
            PreviousStageTrackingInfo = CurrentStageTrackingInfo;
        }

        public void BeginInputMessage(IMessage message)
        {
            CurrentMessageSequence = 0;
            CurrentInputMessage = message;
        }

        public void TrackStageOutputMessage(IMessage message)
        {
            TrackingInfo previousTrackingInfo = PreviousStageTrackingInfo[CurrentInputMessage];
            TrackingInfo newTrackingInfo = previousTrackingInfo.GetNextInSequence(CurrentStageName, CurrentMessageSequence);
            MessageTracker.TrackMessage(newTrackingInfo, message);
            CurrentStageTrackingInfo.Add(message, newTrackingInfo);
            CurrentMessageSequence++;
        }
    }
}
