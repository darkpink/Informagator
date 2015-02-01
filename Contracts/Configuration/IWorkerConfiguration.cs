﻿using System;
using System.Collections.Generic;

namespace Informagator.Contracts.Configuration
{
    public interface IWorkerConfiguration : IConfigurableType
    {
        string Name { get; }
        
        IList<IStageConfiguration> Stages { get; }

        IList<IErrorHandlerConfiguration> ErrorHandlers { get; }
    }
}
