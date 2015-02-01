using Informagator.CommonComponents.Messages;
using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.SupplierStages
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

        public IMessage Supply()
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


        public string ReceviedFrom
        {
            get { return "MSMQ " + QueueName; }
        }

        public string Name
        {
            get { return "TransactionalMsmqBinarySupplier"; }
        }


        public void ValidateSettings()
        {
            //TODO
        }


        public bool IsBlocking
        {
            get { return false; }
        }


        public void Reply(IMessage reply)
        {
            throw new ConfigurationException("Cannot reply to MSMQ");
        }

        public void Consumed()
        {
            //TODO: this is the appropriate time to delete the message from the queue
        }
    }
}
