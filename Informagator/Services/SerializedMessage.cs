using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Services
{
    [DataContract]
    public sealed class SerializedMessage : IMessage
    {
        [DataMember]
        public byte[] BinaryData { get; set;}

        [DataMember]
        public Dictionary<string, string> Attributes { get; set; }

        [DataMember]
        public List<string> ProcessingTrail { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        IDictionary<string, string> IMessage.Attributes
        {
            get { return Attributes; }
        }

        IList<string> IMessage.ProcessingTrail
        {
            get { return ProcessingTrail; }
        }

        public SerializedMessage(IMessage message)
        {
            BinaryData = message.BinaryData;
            Attributes = new Dictionary<string, string>(message.Attributes);
            ProcessingTrail = new List<string>(message.ProcessingTrail);
            Id = message.Id;
        }
    }
}
