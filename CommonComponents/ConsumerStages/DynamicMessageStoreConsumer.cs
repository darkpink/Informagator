using Informagator.Configuration;
using Informagator.Exceptions;
using Informagator.Contracts;
using Informagator.Messages;
using Informagator.Stages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.ConsumerStages
{
    public class DynamicMessageStoreConsumer : IConsumerStage
    {
        [ConfigurationParameter]
        public string QueueNameAttribute { get; set; }
        
        [HostProvided]
        public IMessageStore MessageStore { get; set; }

        public string Consume(IMessage message)
        {
            ValidateSettings();
            var queueName = message.Attributes[QueueNameAttribute];
            MessageStore.Enqueue(queueName, message);
            return "MessageStore queue " + queueName;
        }

        public void ValidateSettings()
        {
            if (MessageStore == null)
            {
                throw new InformagatorInvalidOperationException("MessageStore must be provided to DynamicMessageStoreConsumer");
            }

            if (String.IsNullOrEmpty(QueueNameAttribute))
            {
                throw new ConfigurationException("QueueNameAttribute must be set for DynamicMessageStoreConsumer");
            }
        }

        public string Name
        {
            get { return "DynamicMessageStoreConsumer"; }
        }
    }
}
