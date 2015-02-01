using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Machine
{
    public class DefaultMachine : IMachine
    {
        public string MachineName { get; protected set; }
        
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

            MachineName = machineName ?? DetectMachineName();
        }

        protected virtual string DetectMachineName()
        {
            string result = null;

            result = Environment.MachineName;

            return result;
        }

        public void Start()
        {
            Configuration = ConfigurationProvider.GetMachineConfiguration(MachineName);
            LaunchControlService();
            LaunchInfoService();
            BuildThreads();
        }

        public void UpdateConfiguration()
        {
            IMachineConfiguration newConfiguration = ConfigurationProvider.GetMachineConfiguration(MachineName);
            ConfigurationChangeEvaluator evaulator = new ConfigurationChangeEvaluator();

            foreach (string removedThreadName in Configuration.Workers.Keys.Except(newConfiguration.Workers.Keys))
            {
                DestroyThread(removedThreadName);
            }

            foreach (string existingThreadName in newConfiguration.Workers.Keys.Intersect(Configuration.Workers.Keys))
            {
                if (evaulator.IsRestartRequired(Threads[existingThreadName], Configuration.Workers[existingThreadName], newConfiguration.Workers[existingThreadName]))
                {
                    //TODO: if the thread was stopped, don't start it
                    DestroyThread(existingThreadName);
                    AddThread(newConfiguration.Workers[existingThreadName]);
                }
            }

            foreach (string newThreadName in newConfiguration.Workers.Keys.Except(Configuration.Workers.Keys))
            {
                AddThread(newConfiguration.Workers[newThreadName]);
            }

            Configuration = newConfiguration;
        }

        private void AddThread(IWorkerConfiguration threadConfiguration)
        {
            var thread = new HostIsolator(MachineName, threadConfiguration);
            Threads.Add(threadConfiguration.Name, thread);
            thread.Start();
        }

        private void DestroyThread(string removedThreadName)
        {
            Threads[removedThreadName].Stop();
            Threads.Remove(removedThreadName);
        }

        private void LaunchInfoService()
        {
            RemoteInfoService = new InfoService(this);
            InfoServiceHost.StartService(RemoteInfoService, Configuration.InfoServicePort);
        }

        private void LaunchControlService()
        {
            RemoteAdminService = new AdminService(this);
            AdminServiceHost.StartService(RemoteAdminService, Configuration.AdminServicePort);
        }

        private void BuildThreads()
        {
            Threads = new Dictionary<string, HostIsolator>();

            Configuration = ConfigurationProvider.GetMachineConfiguration(MachineName);

            foreach (string threadName in Configuration.Workers.Keys)
            {
                AddThread(Configuration.Workers[threadName]);
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
