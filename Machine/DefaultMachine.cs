using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Machine
{
    public class DefaultMachine : IMachine
    {
        internal Dictionary<string, HostIsolator> Threads { get; set; }

        protected IMachineConfiguration Configuration { get; set; }

        protected IConfigurationProvider ConfigurationProvider { get; set; }

        protected IAssemblyProvider AssemblySource { get; set; }

        private InfoService RemoteInfoService { get; set; }

        private AdminService RemoteAdminService { get; set; }

        protected IUnityContainer Container { get; set; }

        public DefaultMachine(string machineName = null)
        {
            Container = new UnityContainer();
            Container.LoadConfiguration();

            AssemblySource = Container.Resolve<IAssemblyProvider>();
            ConfigurationProvider = Container.Resolve<IConfigurationProvider>();
        }

        public void Start()
        {
            Configuration = ConfigurationProvider.Configuration;
            LaunchControlService();
            LaunchInfoService();
            BuildThreads();
            StartThreads();
        }

        public void ReloadConfiguration()
        {
            IMachineConfiguration newConfiguration = ConfigurationProvider.Configuration;

            foreach (string newThreadName in newConfiguration.ThreadConfiguration.Keys.Except(Configuration.ThreadConfiguration.Keys))
            {
            }

            foreach (string removedThreadName in Configuration.ThreadConfiguration.Keys.Except(newConfiguration.ThreadConfiguration.Keys))
            {
            }

            foreach (string existingThreadName in newConfiguration.ThreadConfiguration.Keys.Intersect(Configuration.ThreadConfiguration.Keys))
            {
                IThreadConfiguration newThreadConfig = newConfiguration.ThreadConfiguration[existingThreadName];
                IThreadConfiguration oldThreadConfig = Configuration.ThreadConfiguration[existingThreadName];
                if (!newThreadConfig.IsSameAs(oldThreadConfig))
                {
                    HostIsolator thread = Threads[existingThreadName];
                    thread.Stop();
                    Threads.Remove(existingThreadName);
                    thread = new HostIsolator();
                    thread.Configuration = newThreadConfig;
                    thread.Start();
                    Threads.Add(existingThreadName, thread);
                }
            }

            Configuration = newConfiguration;
        }

        private void LaunchInfoService()
        {
            RemoteInfoService = new InfoService(this);
            InfoServiceHost.StartService(RemoteInfoService, Configuration.InfoServiceAddress, Configuration.InfoServicePort);
        }

        private void LaunchControlService()
        {
            RemoteAdminService = new AdminService(this);
            AdminServiceHost.StartService(RemoteAdminService, Configuration.AdminServiceAddress, Configuration.AdminServicePort);
        }

        private void StartThreads()
        {
            foreach (HostIsolator thread in Threads.Values)
            {
                thread.Start();
            }
        }

        private void BuildThreads()
        {
            Threads = new Dictionary<string, HostIsolator>();

            Configuration = ConfigurationProvider.Configuration;

            foreach (string threadName in Configuration.ThreadConfiguration.Keys)
            {
                var thread = new HostIsolator();
                thread.Configuration = Configuration.ThreadConfiguration[threadName];
                Threads.Add(threadName, thread);
            }
        }

        public void Stop()
        {
            StopControlService();
            StopInfoService();
            StopThreads();
        }

        private void StopThreads()
        {
            foreach (HostIsolator host in Threads.Values)
            {
                host.Stop();
            }
        }

        private void StopInfoService()
        {
            InfoServiceHost.StopService();
        }

        private void StopControlService()
        {
            AdminServiceHost.StopService();
        }

        public void StopThread(string threadName)
        {
            Threads[threadName].Stop();
        }

        public void StartThread(string threadName)
        {
            Threads[threadName].Start();
        }
    }
}
