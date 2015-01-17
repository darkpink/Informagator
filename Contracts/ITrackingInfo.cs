using System;

namespace Informagator.Contracts
{
    public interface ITrackingInfo
    {
        string Exception { get; set; }
        ITrackingInfo GetNextInSequence(string stage, int messageSequence);
        int MessageSequence { get; set; }
        Guid ProcessingSequenceId { get; set; }
        string Stage { get; set; }
        int StageSequence { get; set; }
        DateTime TrackDateTime { get; set; }
    }
}
