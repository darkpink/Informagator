using System;
using System.Collections.Generic;

namespace Informagator.Contracts.Configuration
{
    public interface IStageConfiguration
    {
        string ErrorHandlerAssemblyName { get; set; }
        string ErrorHandlerType { get; set; }
        bool IsSameAs(IStageConfiguration config);
        bool IsTrackingEnabled { get; set; }
        IList<IStageConfigurationParameter> Parameters { get; set; }
        string StageAssemblyName { get; set; }
        string StageType { get; set; }
    }
}
