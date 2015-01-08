using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Messages
{
    public abstract class MessageBase<TBody> : IMessage<TBody>
    {
        private Dictionary<string, string> _attributes;
        private List<string> _processingTrail;

        public Guid Id { get; protected set; }

        public TBody Body { get; set;}

        public abstract byte[] BinaryData { get; set; }

        public IDictionary<string, string> Attributes
        {
            get { return _attributes; }
        }

        public IList<string> ProcessingTrail
        {
            get { return _processingTrail; }
        }

        public MessageBase()
        {
            _processingTrail = new List<string>();
            _attributes = new Dictionary<string,string>();
            Id = Guid.NewGuid();
        }
    }
}
