using System;

namespace Informagator.Contracts
{
    public interface IMessageTrackingInfo
    {
        string MachineName { get; }

        string WorkerName { get; }

        Guid ProcessingSequenceId { get; }

        string StageName { get; }

        int StageSequence { get; }

        int? InputMessageSequence { get; }  //ordinal of the message passed into the stage (null for the supplier)

        int OutputMessageSequence { get; }  //ordinal of the messae passed out, unique (does not zero out for each input)

        string Info { get; set; }
        
        string Exception { get; }
        
        DateTime TrackDateTime { get; }
    }
}
