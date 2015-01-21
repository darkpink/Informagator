using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;
using Informagator.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.SystemStatus
{
    public class AutoRefreshingSystemStatus : ObservableCollection<AutoRefreshingThreadStatus>
    {
        protected IConfigurationProvider ConfigurationProvider { get; set; }

        protected Dictionary<string, IMachineConfiguration> ConfigurationCache { get; set; }
        public AutoRefreshingSystemStatus(IConfigurationProvider configurationProvider)
            : base()
        {
            ConfigurationCache = new Dictionary<string, IMachineConfiguration>();
            ConfigurationProvider = configurationProvider;
            ReloadConfiguration();
        }

        public async void ReloadConfiguration()
        {
            Task syncTack = new Task(() => SyncronizeThreadsWithCurrentConfiguration());
            syncTack.Start();
            await syncTack;
            foreach(AutoRefreshingThreadStatus status in this)
            {
                UpdateThreadStatus(status);
            }
        }

        private void UpdateThreadStatus(AutoRefreshingThreadStatus status)
        {
            IMachineConfiguration machineConfig = ConfigurationCache[status.MachineName];
            int infoServicePort = machineConfig.InfoServicePort;
            string url = InfoServiceAddress.Format(machineConfig.IPAddress, infoServicePort);
            status.UpdateFromService(url);
        }

        private void SyncronizeThreadsWithCurrentConfiguration()
        {
            var machineThreads = new Dictionary<string, List<string>>();

            string[] machineNames = ConfigurationProvider.GetActiveMachineNames().ToArray();

            ConfigurationCache.Clear();

            foreach (string machine in machineNames)
            {
                IMachineConfiguration machineConfiguration = ConfigurationProvider.GetMachineConfiguration(machine);
                ConfigurationCache.Add(machine, machineConfiguration);
                machineThreads.Add(machine, machineConfiguration.ThreadConfiguration.Keys.ToList());
            }

            var machineThreadNamePairs = machineThreads.SelectMany(kvp => kvp.Value.Select(threadName => new { MachineName = kvp.Key, ThreadName = threadName }));
            var newMachineThreadNamePairs = machineThreadNamePairs.Where(mtnp => !this.Any(status => status.MachineName == mtnp.MachineName && status.ThreadName == mtnp.ThreadName));
            foreach (var machineThreadNamePair in newMachineThreadNamePairs)
            {
                Add(new AutoRefreshingThreadStatus() { MachineName = machineThreadNamePair.MachineName, ThreadName = machineThreadNamePair.ThreadName });
            }

            var deletedThreads = this.Where(status => !machineThreadNamePairs.Any(mtnp => mtnp.MachineName == status.MachineName && status.ThreadName == mtnp.ThreadName));
            foreach (AutoRefreshingThreadStatus status in deletedThreads)
            {
                this.Remove(status);
            }
        }
    }
}
