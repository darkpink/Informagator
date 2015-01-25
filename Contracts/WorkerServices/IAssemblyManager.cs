using System;

namespace Informagator.Contracts.WorkerServices
{
    public interface IAssemblyManager
    {
        System.Reflection.Assembly GetAssembly(string name);

        bool AnyAssemblyChanged { get; }
    }
}
