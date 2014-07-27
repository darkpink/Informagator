using Acadian.Informagator.Configuration;
using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.CommonComponents.SupplierStages
{
    public class TransactionalMsmqBinarySupplier : ISupplierStage
    {
        private readonly TimeSpan ReceiveTimeout = TimeSpan.FromSeconds(1);
        protected MessageQueue Queue { get; set; }

        protected string _queueName;

        [ConfigurationParameter]
        public string QueueName
        {
            get
            {
                return _queueName;
            }
            set
            {
                _queueName = value;
                ReopenQueue();
            }
        }

        protected void ReopenQueue()
        {
            if (Queue != null)
            {
                Queue.Dispose();
            }

            Queue = new MessageQueue(QueueName, QueueAccessMode.Receive);
        }

        public IMessage GetMessage()
        {
            ByteArrayMessage result = null;

            try
            {
                MessageQueueTransaction trans = new MessageQueueTransaction();
                trans.Begin();
                Message msg = Queue.Receive(ReceiveTimeout, trans);

                if (msg != null)
                {
                    result = new ByteArrayMessage(msg.BodyStream);
                    msg.Dispose();
                }

                trans.Commit();
            }
            catch (MessageQueueException)
            {
                ReopenQueue();
            }

            return result;
        }
    }
}
