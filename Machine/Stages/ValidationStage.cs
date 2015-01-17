using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Stages
{
    public abstract class ValidationStage : ProcessingStage
    {
        public ValidationStage(IMessageErrorHandler errorHandler)
            : base(errorHandler)
        {
        }

        protected sealed override IMessage Perform(IMessage message)
        {
            Validate(message);

            return message;
        }
        
        protected abstract void Validate(IMessage message);
    }
}
