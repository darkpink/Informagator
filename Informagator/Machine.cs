using Informagator.Configuration;
using Informagator.Contracts;
using Informagator.Messages;
using Informagator.Services;
using Informagator.Threads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator
{
    public class Machine
    {
        protected IMessageStore MessageStore { get; set; }

        internal Dictionary<string, Isolator> Threads { get; set; }

        protected IInformagatorConfiguration Configuration { get; set; }

        protected IConfigurationProvider ConfigurationProvider { get; set; }

        protected IMessageTracker MessageTracker { get; set; }

        protected IAssemblySource AssemblySource { get; set; }

        private InfoService RemoteInfoService { get; set; }

        private AdminService RemoteAdminService { get; set; }

        public Machine(IConfigurationProvider configurationProvider, IAssemblySource assemblySource, IMessageStore messageStore, IMessageTracker messageTracker)
        {
            AssemblySource = assemblySource;
            ConfigurationProvider = configurationProvider;
            MessageStore = messageStore;
            MessageTracker = messageTracker;
        }

        public void Start()
        {
            Configuration = ConfigurationProvider.Configuration;
            LaunchControlService();
            LaunchInfoService();
            LaunchInternalService();
            BuildThreads();
            StartThreads();
        }

        public void ReloadConfiguration()
        {
            IInformagatorConfiguration newConfiguration = ConfigurationProvider.Configuration;

            foreach (string newThreadName in newConfiguration.ThreadConfiguration.Keys.Except(Configuration.ThreadConfiguration.Keys))
            {
            }

            foreach (string removedThreadName in Configuration.ThreadConfiguration.Keys.Except(newConfiguration.ThreadConfiguration.Keys))
            {
            }

            foreach (string existingThreadName in newConfiguration.ThreadConfiguration.Keys.Intersect(Configuration.ThreadConfiguration.Keys))
            {
                ThreadConfiguration newThreadConfig = newConfiguration.ThreadConfiguration[existingThreadName];
                ThreadConfiguration oldThreadConfig = Configuration.ThreadConfiguration[existingThreadName];
                if (!newThreadConfig.IsSameAs(oldThreadConfig))
                {
                    Isolator thread = Threads[existingThreadName];
                    thread.Stop();
                    Threads.Remove(existingThreadName);
                    thread = new Isolator();
                    thread.Name = existingThreadName;
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

        private void LaunchInternalService()
        {
            InternalService internalService = new InternalService(MessageStore, MessageTracker, AssemblySource, ConfigurationProvider);
            InternalServiceHost.StartService(internalService);
        }

        private void StartThreads()
        {
            foreach (Isolator thread in Threads.Values)
            {
                thread.Start();
            }
        }

        private void BuildThreads()
        {
            Threads = new Dictionary<string, Isolator>();

            Configuration = ConfigurationProvider.Configuration;

            foreach (string threadName in Configuration.ThreadConfiguration.Keys)
            {
                var thread = new Isolator();
                thread.MessageStore = MessageStore;
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
            foreach (Isolator host in Threads.Values)
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
