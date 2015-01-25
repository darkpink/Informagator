using System;
using System.Collections.Generic;

namespace Informagator.Contracts.Configuration
{
    public interface IThreadConfiguration
    {
        string Name { get; }
        
        IList<IStageConfiguration> StageConfigurations { get; }

        IList<IWorkerConfigurationParameter> WorkerConfigurationParameters { get; }

        string WorkerClassTypeAssembly { get; }
        
        string WorkerClassTypeName { get; }
    }
}
