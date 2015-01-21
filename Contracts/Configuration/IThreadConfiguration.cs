using System;
using System.Collections.Generic;

namespace Informagator.Contracts.Configuration
{
    public interface IThreadConfiguration
    {
        bool IsSameAs(IThreadConfiguration config);
        string Name { get; set; }
        IList<IStageConfiguration> StageConfigurations { get; set; }
        string WorkerClassTypeAssembly { get; set; }
        string WorkerClassTypeName { get; set; }
    }
}
