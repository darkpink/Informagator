﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    public interface IObserverStage : IProcessingStage
    {
        void Observe(IMessage message);
    }
}
