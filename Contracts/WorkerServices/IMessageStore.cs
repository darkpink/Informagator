using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.WorkerServices
{
    public interface IMessageStore
    {
        void Enqueue(string queueName, IMessage message);

        IMessage Dequeue(string queueName);
    }
}
