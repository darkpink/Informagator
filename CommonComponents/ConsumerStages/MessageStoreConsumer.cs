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
    public class MessageStoreConsumer : IConsumerStage
    {
        [ConfigurationParameter]
        public string QueueName { get; set; }
        
        [HostProvided]
        public IMessageStore MessageStore { get; set; }

        public void Consume(IMessage message)
        {
            ValidateSettings();
            MessageStore.Enqueue(QueueName, message);
        }

        protected void ValidateSettings()
        {
            if (MessageStore == null)
            {
                throw new InformagatorInvalidOperationException("MessageStore must be provided to MessageStoreConsumer");
            }

            if (String.IsNullOrEmpty(QueueName))
            {
                throw new ConfigurationException("QueueName must be set for MessageStoreConsumer");
            }
        }

        public string SentTo
        {
            get { return "MessageStore queue " + QueueName; }
        }

        public string Name
        {
            get { return "MessageStoreConsumer"; }
        }
    }
}
