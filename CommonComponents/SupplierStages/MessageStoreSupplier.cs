using Acadian.Informagator.Configuration;
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
    public class MessageStoreQueueSource : SupplierStage
    {
        public MessageStoreQueueSource(IMessageErrorHandler errorHandler)
            : base(errorHandler)
        {
        }

        [ConfigurationParameter]
        public string QueueName { get; set; }

        [Import]
        public IMessageStore MessageStore { get; set; }


        protected override IMessage GetMessage()
        {
            IMessage result = MessageStore.Dequeue(QueueName);

            return result;
        }

    }
}
