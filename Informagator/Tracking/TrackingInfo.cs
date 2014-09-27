using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Tracking
{
    [DataContract]
    public class TrackingInfo
    {
        [DataMember]
        public Guid ProcessingSequenceId { get; set; }
        
        [DataMember]
        public int StageSequence { get; set; }

        [DataMember]
        public DateTime TrackDateTime { get; set; }

        [DataMember]
        public string Stage { get; set; }

        [DataMember]
        public string ReceivedFrom { get; set; }

        [DataMember]
        public string SentTo { get; set; }
    }
}
