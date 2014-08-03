using System;
namespace Acadian.Informagator.Infrastructure
{
    public interface IStageConfigurationParameter
    {
        bool IsSameAs(IStageConfigurationParameter param);
        string Name { get; }
        string Value { get; }
    }
}
