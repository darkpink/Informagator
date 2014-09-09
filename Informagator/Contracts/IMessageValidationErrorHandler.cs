using Acadian.Informagator.Exceptions;
using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    public interface IMessageValidationErrorHandler
    {
        void Handle(IMessage message, MessageValidationException validationResult);
    }
}
