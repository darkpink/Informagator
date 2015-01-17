using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.Tracking
{
    public class TrackingInfo : ITrackingInfo
    {
        public TrackingInfo(Guid processingSequenceId, int stageSequence, string stage, int messageSequence)
        {
            ProcessingSequenceId = processingSequenceId;
            Stage = stage;
            MessageSequence = messageSequence;
            StageSequence = stageSequence;
            TrackDateTime = DateTime.Now;
        }

        public Guid ProcessingSequenceId { get; set; }   //guid for the processing sequence - same id from load/receive to save/transmit
        
        public int StageSequence { get; set; }  //serial sequence number for the stage within the processing sequence

        public int MessageSequence { get; set; } //for a stage, which # message of the batch is processing (only applies when a transform creates multiple messages)

        public DateTime TrackDateTime { get; set; }

        public string Stage { get; set; }  //stage name

        public string Exception { get; set; }

        public ITrackingInfo GetNextInSequence(string stage, int messageSequence)
        {
            TrackingInfo result = new TrackingInfo(ProcessingSequenceId, StageSequence + 1, stage, messageSequence);
            result.TrackDateTime = DateTime.Now;

            return result;
        }
    }
}
