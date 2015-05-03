using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.Tracking
{
    public class MessageTrackingInfo : IMessageTrackingInfo
    {
        public string MachineName { get; protected set; }

        public string WorkerName { get; protected set; }

        public Guid ProcessingSequenceId { get; protected set; }   //guid for the processing sequence - same id from load/receive to save/transmit

        public string StageName { get; protected set; }

        public int StageSequence { get; protected set; }  //serial sequence number for the stage within the processing sequence

        public int? InputMessageSequence { get; protected set; }

        public int OutputMessageSequence { get; protected set; }

        public DateTime TrackDateTime { get; protected set; }

        public string Exception { get; protected set; }

        public string Info { get; set; }
        
        public MessageTrackingInfo(string machineName, string workerName, Guid processingSequenceId, int stageSequence, 
                            string stageName, int? inputMessageSequence, int outputMessageSequence)
        {
            MachineName = machineName;
            WorkerName = workerName;
            ProcessingSequenceId = processingSequenceId;
            StageName = stageName;
            StageSequence = stageSequence;
            InputMessageSequence = inputMessageSequence;
            OutputMessageSequence = outputMessageSequence;
            TrackDateTime = DateTime.Now;
        }
    }
}
