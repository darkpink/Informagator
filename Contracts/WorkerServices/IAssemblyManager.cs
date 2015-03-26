using Informagator.Contracts.Configuration;
using System;

namespace Informagator.Contracts.WorkerServices
{
    public interface IAssemblyManager
    {
        System.Reflection.Assembly GetAssembly(string name, string version);

        bool AnyAssemblyChanged { get; }

        object CreateConfiguredObject(IConfigurableTypeConfiguration type, object host);
    }
}
