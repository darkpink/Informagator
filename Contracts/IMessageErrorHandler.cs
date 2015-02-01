using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IMessageErrorHandler
    {
        void Handle(IList<string> info, Exception ex, IMessage message);

        void ValidateSettings();

        IList<string> ContextInfo { set; }
    }
}
