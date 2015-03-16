using System;
using System.Collections.Generic;

namespace Informagator.Contracts.Configuration
{
    public interface IStageConfiguration : IConfigurableType
    {
        bool SuppressWorkerErrorHandlers { get; }

        IList<IErrorHandlerConfiguration> ErrorHandlers { get; }
    }
}
