using Acadian.Informagator.Configuration;
using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.CommonComponents.ConsumerStages
{
    public class TransactionalMsmqBinaryConsumer : IProcessingStage
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

        public IMessage Execute(IMessage msgIn)
        {
            try
            {
                MessageQueueTransaction trans = new MessageQueueTransaction();
                trans.Begin();
                Message msg = new Message();
                msg.BodyStream = new MemoryStream(msgIn.BinaryData);

                Queue.Send(msg, "Informagator message " + MessageCount++, trans);
                trans.Commit();
            }
            catch(MessageQueueException)
            {
                ReopenQueue();
            }
            
            return null;
        }
    }
}
