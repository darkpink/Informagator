using Acadian.Informagator.Configuration;
using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Messages;
using Acadian.Informagator.Services;
using Acadian.Informagator.Threads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator
{
    public class InformagatorService
    {
        public IMessageStore MessageStore { get; set; }

        protected List<IInformagatorThreadIsolator> Threads { get; set; }

        protected IInformagatorConfiguration Configuration { get; set; }

        protected IConfigurationProvider ConfigurationProvider { get; set; }

        protected IAssemblySource AssemblySource { get; set; }

        public InformagatorService(IConfigurationProvider configurationProvider, IAssemblySource assemblySource, IMessageStore messageStore)
        {
            AssemblySource = assemblySource;
            ConfigurationProvider = configurationProvider;
            MessageStore = messageStore;
        }

        public void Start()
        {
            LaunchControlService();
            LaunchInfoService();
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
                IThreadConfiguration newThreadConfig = newConfiguration.ThreadConfiguration[existingThreadName];
                IThreadConfiguration oldThreadConfig = Configuration.ThreadConfiguration[existingThreadName];
                if (!newThreadConfig.IsSameAs(oldThreadConfig))
                {
                    IInformagatorThreadIsolator thread = Threads.Single(i => i.ThreadConfiguration.Name == newThreadConfig.Name);
                    thread.Stop();
                    Threads.Remove(thread);
                    thread = new Isolator(newConfiguration.ThreadConfiguration[existingThreadName], AssemblySource);
                    thread.Start();
                    Threads.Add(thread);
                }
            }

            Configuration = newConfiguration;
        }

        private void LaunchInfoService()
        {
            InfoService remoteControlService = new InfoService();
            InfoServiceHost.StartService(remoteControlService);
        }

        private void LaunchControlService()
        {
            AdminService remoteControlService = new AdminService();
            AdminServiceHost.StartService(remoteControlService);
        }

        private void StartThreads()
        {
            foreach (IInformagatorThreadIsolator thread in Threads)
            {
                thread.Start();
            }
        }

        private void BuildThreads()
        {
            var threads = new List<IInformagatorThreadIsolator>();

            Configuration = ConfigurationProvider.Configuration;

            foreach (string threadName in Configuration.ThreadConfiguration.Keys)
            {
                var thread = new Isolator(Configuration.ThreadConfiguration[threadName], AssemblySource);
                threads.Add(thread);
            }

            Threads = threads;
        }

        public void Stop()
        {
            StopControlService();
            StopInfoService();
            StopThreads();
        }

        private void StopThreads()
        {
            foreach (IInformagatorThreadIsolator host in Threads)
            {
                host.Stop();
            }
        }

        private void StopInfoService()
        {
        }

        private void StopControlService()
        {
        }

    }
}
