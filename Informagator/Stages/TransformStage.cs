using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acadian.Informagator.Messages;
using Acadian.Informagator.Infrastructure;

namespace Acadian.Informagator.Stages
{
    public abstract class TransformStage : ProcessingStage
    {
        public TransformStage(IMessageErrorHandler errorHandler)
            : base(errorHandler)
        {
        }

        protected sealed override IMessage Perform(IMessage message)
        {
            return TransformMessage(message);
        }

        protected abstract IMessage TransformMessage(IMessage message);
    }
}
