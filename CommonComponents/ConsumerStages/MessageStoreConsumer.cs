using Acadian.Informagator.Configuration;
using Acadian.Informagator.Exceptions;
using Acadian.Informagator.Infrastructure;
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
    public class MessageStoreConsumer : IProcessingStage
    {
        [ConfigurationParameter]
        public string QueueName { get; set; }
        
        [InformagatorProvided]
        public IMessageStore MessageStore { get; set; }

        public IMessage Execute(IMessage msgIn)
        {
            ValidateSettings();
            MessageStore.Enqueue(QueueName, msgIn);
            
            return null;
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
    }
}
