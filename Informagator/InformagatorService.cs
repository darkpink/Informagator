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

        protected Dictionary<string, IInformagatorThreadIsolator> Threads { get; set; }

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
                var thread = new Isolator();
                thread.ThreadConfiguration = Configuration.ThreadConfiguration[newThreadName];
                thread.AssemblySource = AssemblySource;
                Threads.Add(newThreadName, thread);
            }

            foreach (string removedThreadName in Configuration.ThreadConfiguration.Keys.Except(newConfiguration.ThreadConfiguration.Keys))
            {
                IInformagatorThreadIsolator thread = Threads[removedThreadName];
                thread.Stop();
                Threads.Remove(removedThreadName);
            }

            foreach (string existingThreadName in newConfiguration.ThreadConfiguration.Keys.Intersect(Configuration.ThreadConfiguration.Keys))
            {
                IThreadConfiguration newThreadConfig = newConfiguration.ThreadConfiguration[existingThreadName];
                IThreadConfiguration oldThreadConfig = Configuration.ThreadConfiguration[existingThreadName];
                if (!newThreadConfig.IsSameAs(oldThreadConfig))
                {
                    IInformagatorThreadIsolator thread = Threads[existingThreadName];
                    thread.Stop();
                    thread.AssemblySource = AssemblySource;
                    thread.ThreadConfiguration = newConfiguration.ThreadConfiguration[existingThreadName];
                    thread.Start();
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
            foreach (IInformagatorThreadIsolator thread in Threads.Values)
            {
                thread.Start();
            }
        }

        private void BuildThreads()
        {
            Threads = new Dictionary<string, IInformagatorThreadIsolator>();
            Configuration = ConfigurationProvider.Configuration;

            foreach (string threadName in Configuration.ThreadConfiguration.Keys)
            {
                var thread = new Isolator();
                thread.ThreadConfiguration = Configuration.ThreadConfiguration[threadName];
                thread.AssemblySource = AssemblySource;
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
            foreach (IInformagatorThreadIsolator host in Threads.Values)
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
    }
}
