using System;
using System.Collections.Generic;

namespace Informagator.Contracts.Configuration
{
    public interface IWorkerConfiguration : IConfigurableTypeConfiguration
    {
        string Name { get; }
        
        IList<IStageConfiguration> Stages { get; }

        bool SuppressParentErrorHandlers { get; }

        bool AutoStart { get; }

        IList<IErrorHandlerConfiguration> ErrorHandlers { get; }
    }
}
