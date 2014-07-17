using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.CommonComponents.ErrorHandlers
{
    public class IgnoreErrorHandler : IMessageErrorHandler
    {
        public void Handle(IMessage message, Exception ex)
        {
            
        }
    }
}
