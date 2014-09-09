using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    [ServiceContract]

    public interface IMessageStore
    {
        [OperationContract]
        void Enqueue(string queueName, IMessage message);

        [OperationContract]
        IMessage Dequeue(string queueName);
    }
}
