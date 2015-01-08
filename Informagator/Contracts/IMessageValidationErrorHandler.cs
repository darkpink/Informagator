using Informagator.Exceptions;
using Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IMessageValidationErrorHandler
    {
        void Handle(IMessage message, MessageValidationException validationResult);
    }
}
