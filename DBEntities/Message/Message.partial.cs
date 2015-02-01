using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DBEntities.Message
{
    public partial class Message : IMessage
    {
        Guid IMessage.Id
        {
            get 
            { 
                return MessageId; 
            }
        }

        public byte[] BinaryData
        {
            get
            {
                return Encoding.ASCII.GetBytes(Body);
            }
            set
            {
                Body = Encoding.ASCII.GetString(value);
            }
        }

        public IDictionary<string, string> Attributes
        {
            get 
            { 
                return Attributes.ToDictionary(attr => attr.Key, attr => attr.Value); 
            }
        }

        public IList<string> ProcessingTrail
        {
            get 
            { 
                return new List<string>() { String.Format("Retrieved from DB, id={0}, queue={1}", Id, QueueName) }; 
            }
        }
    }
}
