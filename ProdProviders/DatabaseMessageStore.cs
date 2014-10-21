using Acadian.Informagator.Contracts;
using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.ProdProviders
{
    public class DatabaseMessageStore : IMessageStore
    {
        public void Enqueue(string queueName, IMessage message)
        {
            using (MessageEntities entities = new MessageEntities())
            {
                Message m = entities.Messages.Create();
                m.Body = Encoding.ASCII.GetString(message.BinaryData);
                m.QueueName = queueName;
                entities.SaveChanges();
            }
        }

        public IMessage Dequeue(string queueName)
        {
            ByteArrayMessage result = null;

            using (MessageEntities entities = new MessageEntities())
            {
                long? messageId = entities.Dequeue(queueName).SingleOrDefault();
                if (messageId != null)
                {
                    Message message = entities.Messages.Single(m => m.Id == messageId);
                    result = new ByteArrayMessage();
                    result.Body = Encoding.ASCII.GetBytes(message.Body);
                }
            }

            return result;
        }
    }
}
