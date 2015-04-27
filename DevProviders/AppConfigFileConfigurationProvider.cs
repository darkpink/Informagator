using Informagator.Contracts.Providers;
using Informagator.DevProviders.DotNetConfiguration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders
{
    public class AppConfigFileConfigurationProvider : IConfigurationProvider
    {
        protected InformagatorConfigurationSection ConfigSection { get; set; } 
        
        public AppConfigFileConfigurationProvider()
        {
            ConfigSection = ConfigurationManager.GetSection("informagator") as InformagatorConfigurationSection;
        }

        public IEnumerable<string> GetActiveMachineNames()
        {
            return ConfigSection.Machines.OfType<Machine>().Select(m => m.HostName);
        }

        public Contracts.Configuration.IMachineConfiguration GetMachineConfiguration(string machineName)
        {
            return ConfigSection.Machines.OfType<Machine>().Where(m => m.HostName == machineName).SingleOrDefault();
        }

        public Contracts.Configuration.IWorkerConfiguration GetThreadConfiguration(string machineName, string threadName)
        {
            return ConfigSection.Machines
                .OfType<Machine>()
                .Where(m => m.HostName == machineName)
                .SelectMany(m => m.Workers.OfType<Worker>())
                .Where(w => w.Name == threadName)
                .SingleOrDefault();
        }
    }
}
