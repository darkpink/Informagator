using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Configuration
{
    public interface IConfigurableTypeConfiguration
    {
        string AssemblyName { get; }

        string AssemblyVersion { get; }

        string Type { get; }

        IList<IConfigurationParameter> Parameters { get; }

    }
}
