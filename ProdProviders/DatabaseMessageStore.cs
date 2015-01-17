using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Informagator.Contracts.WorkerServices;

namespace Informagator.ProdProviders
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
            DatabaseMessage result = null;

            using (MessageEntities entities = new MessageEntities())
            {
                long? messageId = entities.Dequeue(queueName).SingleOrDefault();
                if (messageId != null)
                {
                    Message message = entities.Messages.Include(m => m.MessageAttributes).Single(m => m.Id == messageId);
                    result = new DatabaseMessage(message);
                }
            }

            return result;
        }
    }
}
