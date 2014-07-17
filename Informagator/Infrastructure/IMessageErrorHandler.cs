using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IMessageErrorHandler
    {
        void Handle(IMessage message, Exception ex);
    }
}
