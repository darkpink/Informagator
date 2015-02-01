using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;
using Informagator.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Informagator.SystemStatus
{
    public class AutoRefreshingSystemStatus : ObservableCollection<AutoRefreshingThreadStatus>, IDisposable
    {
        protected IConfigurationProvider ConfigurationProvider { get; set; }

        protected int RefreshIntervalMilliseconds { get; set; }

        protected bool IsDisposed { get; set; }

        protected Dictionary<string, IMachineConfiguration> ConfigurationCache { get; set; }
        public AutoRefreshingSystemStatus(IConfigurationProvider configurationProvider, int refreshIntervalMilliseconds = 10000)
            : base()
        {
            ConfigurationCache = new Dictionary<string, IMachineConfiguration>();
            ConfigurationProvider = configurationProvider;
            RefreshIntervalMilliseconds = refreshIntervalMilliseconds;
            ReloadConfiguration();
            
            IsDisposed = false;
            Task.Run(() => UpdateStatusUntilDisposed());
        }

        private void UpdateStatusUntilDisposed()
        {
            while (!IsDisposed)
            {
                Task.WaitAll(this.Select(threadStatus => UpdateThreadStatus(threadStatus)).ToArray());
                Thread.Sleep(RefreshIntervalMilliseconds);
            }
        }

        public void ReloadConfiguration()
        {
            var machineThreads = new Dictionary<string, List<string>>();

            string[] machineNames = ConfigurationProvider.GetActiveMachineNames().ToArray();

            ConfigurationCache.Clear();

            foreach (string machine in machineNames)
            {
                IMachineConfiguration machineConfiguration = ConfigurationProvider.GetMachineConfiguration(machine);
                ConfigurationCache.Add(machine, machineConfiguration);
                machineThreads.Add(machine, machineConfiguration.Workers.Keys.ToList());
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

        private async Task UpdateThreadStatus(AutoRefreshingThreadStatus status)
        {
            IMachineConfiguration machineConfig = ConfigurationCache[status.MachineName];
            int infoServicePort = machineConfig.InfoServicePort;
            string url = InfoServiceAddress.Format(machineConfig.IPAddress, infoServicePort);
            await Task.Run(() => status.UpdateFromService(url));
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
