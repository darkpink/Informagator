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
    public class StaticMessageStoreConsumer : IConsumerStage
    {
        [ConfigurationParameter]
        public string QueueName { get; set; }
        
        [HostProvided]
        public IMessageStore MessageStore { get; set; }

        public string Consume(IMessage message)
        {
            ValidateSettings();
            MessageStore.Enqueue(QueueName, message);

            return "MessageStore queue " + QueueName;
        }

        public void ValidateSettings()
        {
            if (MessageStore == null)
            {
                throw new InformagatorInvalidOperationException("MessageStore must be provided to StaticMessageStoreConsumer");
            }

            if (String.IsNullOrEmpty(QueueName))
            {
                throw new ConfigurationException("QueueName must be set for StaticMessageStoreConsumer");
            }
        }

        public string Name
        {
            get { return "StaticMessageStoreConsumer"; }
        }
    }
}
