using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Stages
{
    public abstract class SupplierStage : ProcessingStage
    {
        public SupplierStage(IMessageErrorHandler errorHandler)
            : base(errorHandler)
        {
        }

        protected sealed override IMessage Perform(IMessage message)
        {
            return GetMessage();
        }

        protected abstract IMessage GetMessage();

    }
}
