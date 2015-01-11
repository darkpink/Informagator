using Informagator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IMessageErrorHandler
    {
        void Handle(String info, IMessage message, Exception ex);

        void ValidateSettings();
    }
}
