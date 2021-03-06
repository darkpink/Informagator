﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Stages
{
    public interface IConsumerStage : IProcessingStage
    {
        IMessage Consume(IMessage message); //returns either a reply or null
    }
}
