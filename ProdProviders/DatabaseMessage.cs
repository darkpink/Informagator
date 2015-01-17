using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.ProdProviders
{
    internal class DatabaseMessage : IMessage
    {
        private byte[] Body { get; set; }
        
        public Guid Id { get; private set; }

        public IDictionary<string, string> Attributes { get; private set; }
        
        public IList<string> ProcessingTrail { get; private set; }

        public DatabaseMessage(Message data)
        {
            Body = Encoding.ASCII.GetBytes(data.Body);
            Attributes = data.MessageAttributes.ToDictionary(a => a.Attribute, a => a.Value);
            ProcessingTrail = new List<string>() { "Retrieved from DB, id = " + data.Id };
            Id = Guid.NewGuid();
        }

        public byte[] BinaryData
        {
            get
            {
                return Body;
            }
            set
            {
                Body = value;
            }
        }

    }
}
