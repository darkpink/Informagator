using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Tracking
{
    [DataContract]
    public class TrackingInfo
    {
        public TrackingInfo(Guid processingSequenceId, int stageSequence, string stage, int messageSequence)
        {
            ProcessingSequenceId = processingSequenceId;
            Stage = stage;
            MessageSequence = messageSequence;
            StageSequence = stageSequence;
            TrackDateTime = DateTime.Now;
        }

        [DataMember]
        public Guid ProcessingSequenceId { get; set; }   //guid for the processing sequence - same id from load/receive to save/transmit
        
        [DataMember]
        public int StageSequence { get; set; }  //serial sequence number for the stage within the processing sequence

        [DataMember]
        public int MessageSequence { get; set; } //for a stage, which # message of the batch is processing (only applies when a transform creates multiple messages)

        [DataMember]
        public DateTime TrackDateTime { get; set; }

        [DataMember]
        public string Stage { get; set; }  //stage name

        [DataMember]
        public string Exception { get; set; }

        public TrackingInfo GetNextInSequence(string stage, int messageSequence)
        {
            TrackingInfo result = new TrackingInfo(ProcessingSequenceId, StageSequence + 1, stage, messageSequence);
            result.TrackDateTime = DateTime.Now;

            return result;
        }
    }
}
