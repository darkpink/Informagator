using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.ErrorHandlers
{
    [Export(typeof(IMessageErrorHandler))]
    public class IgnoreErrorHandler : IMessageErrorHandler
    {
        public void Handle(IList<string> info, Exception ex, IMessage message)
        {
        }

        public void ValidateSettings()
        {
        }

        public IList<string> ContextInfo { get; set; }
    }
}
