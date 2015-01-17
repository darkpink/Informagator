using System;

namespace Informagator.Contracts.Configuration
{
    public interface IStageConfigurationParameter
    {
        bool IsSameAs(IStageConfigurationParameter param);
        string Name { get; set; }
        string Value { get; set; }
    }
}
