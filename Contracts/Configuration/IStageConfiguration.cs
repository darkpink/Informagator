using System;
using System.Collections.Generic;

namespace Informagator.Contracts.Configuration
{
    public interface IStageConfiguration : IConfigurableTypeConfiguration
    {
        bool SuppressParentErrorHandlers { get; }

        IList<IErrorHandlerConfiguration> ErrorHandlers { get; }
    }
}
