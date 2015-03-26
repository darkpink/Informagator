using System;
using System.Collections.Generic;

namespace Informagator.Contracts.Configuration
{
    public interface IStageConfiguration : IConfigurableTypeConfiguration
    {
        bool SuppressWorkerErrorHandlers { get; }

        IList<IErrorHandlerConfiguration> ErrorHandlers { get; }
    }
}
