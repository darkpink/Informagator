using Acadian.Informagator.Configuration;
using Acadian.Informagator.Exceptions;
using Acadian.Informagator.Contracts;
using Acadian.Informagator.Messages;
using Acadian.Informagator.Stages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.CommonComponents.ConsumerStages
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

        protected void ValidateSettings()
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
