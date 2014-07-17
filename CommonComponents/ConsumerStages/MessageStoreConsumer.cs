using Acadian.Informagator.Configuration;
using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Messages;
using Acadian.Informagator.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.CommonComponents.ConsumerStages
{
    public class MessageStoreConsumer : ConsumerStage
    {
        [ConfigurationParameter]
        public string QueueName { get; set; }
        public IMessageStore MessageStore { get; set; }

        public MessageStoreConsumer(IMessageErrorHandler errorHandler)
            :base(errorHandler)
        {
        }

        protected override void Consume(IMessage message)
        {
            MessageStore.Enqueue(QueueName, message);
        }
    }
}
