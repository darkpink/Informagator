using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IStageConfiguration
    {
        string StageAssemblyName { get; }
        string StageType { get; }
        string ErrorHandlerAssemblyName { get; }
        string ErrorHandlerType { get; }
        bool IsTrackingEnabled { get; set; }
        IList<IStageConfigurationParameter> Parameters { get; }

        bool IsSameAs(IStageConfiguration config);
    }
}
