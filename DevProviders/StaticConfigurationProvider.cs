using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders
{
    public class StaticConfigurationProvider : IConfigurationProvider
    {
        public IMachineConfiguration Configuration { get; protected set; }

        public StaticConfigurationProvider(IMachineConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IMachineConfiguration GetMachineConfiguration(string machineName)
        {
            return Configuration;
        }

        public IEnumerable<string> GetActiveMachineNames()
        {
            return new List<string>();
        }

        public IWorkerConfiguration GetThreadConfiguration(string machineName, string threadName)
        {
            return Configuration.Workers[threadName];
        }
    }
}
