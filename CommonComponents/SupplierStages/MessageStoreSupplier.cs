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

namespace Acadian.Informagator.CommonComponents.SupplierStages
{
    [Export(typeof(ISupplierStage))]
    public class MessageStoreSupplier : ISupplierStage
    {
        [ConfigurationParameter]
        public string QueueName { get; set; }

        [InformagatorProvided]
        public IMessageStore MessageStore { get; set; }

        public IMessage GetMessage()
        {
            IMessage result;

            ValidateSettings();
            result = MessageStore.Dequeue(QueueName);

            return result;
        }

        protected void ValidateSettings()
        {
            if (MessageStore == null)
            {
                throw new InformagatorInvalidOperationException("MessageStore must be provided to MessageStoreSupplier");
            }

            if (String.IsNullOrEmpty(QueueName))
            {
                throw new ConfigurationException("QueueName must be set for MessageStoreSupplier");
            }

        }
    }
}
