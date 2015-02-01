using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.ConsumerStages
{
    public class StaticTransactionalMsmqBinaryConsumer : IConsumerStage
    {
        protected MessageQueue Queue { get; set; }
        
        protected string _queueName;
        protected long MessageCount { get; set; }

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

            Queue = new MessageQueue(QueueName);
        }

        public IMessage Consume(IMessage message)
        {
            try
            {
                MessageQueueTransaction trans = new MessageQueueTransaction();
                trans.Begin();
                Message msg = new Message();
                msg.BodyStream = new MemoryStream(message.BinaryData);

                Queue.Send(msg, "Informagator message " + MessageCount++, trans);
                trans.Commit();
            }
            catch(MessageQueueException)
            {
                ReopenQueue();
            }

            return null;
        }


        public string Name
        {
            get { return "StaticTransactionalMsmqBinaryConsumer"; }
        }


        public void ValidateSettings()
        {
            //TODO
        }
    }
}
