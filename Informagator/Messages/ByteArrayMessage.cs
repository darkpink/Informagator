using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Messages
{
    public class ByteArrayMessage : IMessage
    {
        public Dictionary<string, string> Attributes { get; protected set; }
        public List<string> ProcessingTrail { get; protected set; }

        public Byte[] Body { get; set; }

        public ByteArrayMessage()
        {
            Attributes = new Dictionary<string, string>();
            ProcessingTrail = new List<string>();
        }

        IDictionary<string, string> IMessage.Attributes
        {
            get { return this.Attributes; }
        }

        IList<string> IMessage.ProcessingTrail
        {
            get { return this.ProcessingTrail; }
        }
    }
}
