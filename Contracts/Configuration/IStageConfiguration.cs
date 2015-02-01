using System;
using System.Collections.Generic;

namespace Informagator.Contracts.Configuration
{
    public interface IStageConfiguration : IConfigurableType
    {
        IList<IErrorHandlerConfiguration> ErrorHandlers { get; }
    }
}
