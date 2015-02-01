using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.Configuration
{
    public class HardCodedStageConfiguration : IStageConfiguration
    {
        public string AssemblyName { get; set; }

        public string AssemblyVersion { get; protected set; }

        public string Type { get; set; }

        public IList<IConfigurationParameter> Parameters { get; set; }

        public IList<IErrorHandlerConfiguration> ErrorHandlers { get; protected set; }

        public HardCodedStageConfiguration()
        {
            Parameters = new List<IConfigurationParameter>();
        }
    }
}
