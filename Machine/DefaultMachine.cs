using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.Providers;
using Informagator.Contracts.Services;
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
        public string Name { get; protected set; }
        
        protected Dictionary<string, HostIsolator> Threads { get; set; }

        protected IMachineConfiguration Configuration { get; set; }

        protected IConfigurationProvider ConfigurationProvider { get; set; }

        protected IAssemblyProvider AssemblySource { get; set; }

        protected IInfoService RemoteInfoService { get; set; }

        protected IAdminService RemoteAdminService { get; set; }

        protected IUnityContainer Container { get; set; }

        public DefaultMachine(string machineName = null)
        {
            Container = new UnityContainer();
            Container.LoadConfiguration();

            AssemblySource = Container.Resolve<IAssemblyProvider>();
            ConfigurationProvider = Container.Resolve<IConfigurationProvider>();

            Name = machineName ?? DetectMachineName();
        }

        protected virtual string DetectMachineName()
        {
            string result = null;

            result = Environment.MachineName;

            return result;
        }

        public void Start()
        {
            Configuration = ConfigurationProvider.GetMachineConfiguration(Name);
            LaunchControlService();
            LaunchInfoService();
            BuildThreads();
        }

        public void UpdateConfiguration()
        {
            IMachineConfiguration newConfiguration = ConfigurationProvider.GetMachineConfiguration(Name);
            ConfigurationChangeEvaluator evaulator = new ConfigurationChangeEvaluator();

            foreach (string removedThreadName in Configuration.Workers.Keys.Except(newConfiguration.Workers.Keys))
            {
                DestroyThread(removedThreadName);
            }

            foreach (string existingThreadName in newConfiguration.Workers.Keys.Intersect(Configuration.Workers.Keys))
            {
                if (evaulator.IsRestartRequired(Threads[existingThreadName], Configuration.Workers[existingThreadName], newConfiguration.Workers[existingThreadName]))
                {
                    DestroyThread(existingThreadName);
                    bool wasStopped = Threads[existingThreadName].Status.RunStatus == ThreadRunStatus.Stopped;
                    AddThread(newConfiguration.Workers[existingThreadName], suppressAutoStart: wasStopped);
                }
            }

            foreach (string newThreadName in newConfiguration.Workers.Keys.Except(Configuration.Workers.Keys))
            {
                AddThread(newConfiguration.Workers[newThreadName], suppressAutoStart: false);
            }

            Configuration = newConfiguration;
        }

        private void AddThread(IWorkerConfiguration threadConfiguration, bool suppressAutoStart)
        {
            var thread = new HostIsolator(this, threadConfiguration);
            Threads.Add(threadConfiguration.Name, thread);
            if (!suppressAutoStart && threadConfiguration.AutoStart)
            {
                thread.Start();
            }
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

            Configuration = ConfigurationProvider.GetMachineConfiguration(Name);

            foreach (string threadName in Configuration.Workers.Keys)
            {
                AddThread(Configuration.Workers[threadName], suppressAutoStart: false);
            }
        }

        public void Stop()
        {
            StopControlService();
            StopInfoService();
            StopThreads();
        }

        protected void StopThreads()
        {
            foreach (HostIsolator host in Threads.Values)
            {
                host.Stop();
            }
        }

        protected void StopInfoService()
        {
            InfoServiceHost.StopService();
        }

        protected void StopControlService()
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

        public IThreadStatus GetThreadStatus(string threadName)
        {
            return Threads[threadName].Status;
        }
    }
}
