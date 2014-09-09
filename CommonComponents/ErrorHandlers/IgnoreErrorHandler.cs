﻿using Acadian.Informagator.Contracts;
using Acadian.Informagator.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.CommonComponents.ErrorHandlers
{
    [Export(typeof(IMessageErrorHandler))]
    public class IgnoreErrorHandler : IMessageErrorHandler
    {
        public void Handle(IMessage message, Exception ex)
        {
            
        }
    }
}
