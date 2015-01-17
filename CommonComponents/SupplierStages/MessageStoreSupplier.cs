using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Stages;
using Informagator.Contracts.WorkerServices;

namespace Informagator.CommonComponents.SupplierStages
{
    public class MessageStoreSupplier : ISupplierStage
    {
        [ConfigurationParameter]
        public string QueueName { get; set; }

        [HostProvided]
        public IMessageStore MessageStore { get; set; }

        public IMessage Supply()
        {
            IMessage result;

            ValidateSettings();
            result = MessageStore.Dequeue(QueueName);

            return result;
        }

        public void ValidateSettings()
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

        public string ReceviedFrom
        {
            get { return "MessageStore queue " + QueueName; }
        }

        public string Name
        {
            get { return "MessageStoreSupplier"; }
        }
    }
}
