using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Configuration
{
    public interface IWorkerConfigurationParameter
    {
        string Name { get; }

        string Value { get; }
    }
}
