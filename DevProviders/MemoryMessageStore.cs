using Informagator.Contracts;
using Informagator.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders
{
    [Serializable]
    public class MemoryMessageStore : IMessageStore
    {
        protected Dictionary<string, ConcurrentQueue<IMessage>> Queues = new Dictionary<string, ConcurrentQueue<IMessage>>();

        public void Enqueue(string queueName, IMessage message)
        {
            if (!Queues.ContainsKey(queueName))
            {
                Queues.Add(queueName, new ConcurrentQueue<IMessage>());
            }

            Queues[queueName].Enqueue(message);
        }

        public IMessage Dequeue(string queueName)
        {
            IMessage result = null;

            if (!Queues.ContainsKey(queueName))
            {
                Queues.Add(queueName, new ConcurrentQueue<IMessage>());
            }

            Queues[queueName].TryDequeue(out result);
            
            return result;
        }
    }
}
