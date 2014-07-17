using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Configuration;

namespace Acadian.Informagator.Threads
{
    public class Isolator : IInformagatorThreadIsolator
    {
        public MultigatorThreadStatus Status { get; protected set; }
        public  IThreadConfiguration ThreadConfiguration { get; protected set; }

        protected IInfomagatorThreadHost CrossDomainProxy { get; set; }
        protected AppDomain InnerThreadDomain { get; set; }

        protected IAssemblySource AssemblySource { get; set; }

        public Isolator(IThreadConfiguration configuration, IAssemblySource assemblySource)
        {
            ThreadConfiguration = configuration;
            AssemblySource = assemblySource;
        }

        public void ReloadConfig()
        {
        }

        public void Start()
        {
            InnerThreadDomain = AppDomain.CreateDomain(ThreadConfiguration.Name);
            ThreadHost host = InnerThreadDomain.CreateInstanceAndUnwrap(typeof(ThreadHost).Assembly.FullName, typeof(ThreadHost).FullName) as ThreadHost;
            host.Configuration = ThreadConfiguration;
            host.AssemblyLoader = AssemblySource;
            //AssemblySource.LoadAssembly(ThreadConfiguration.ThreadStartTypeAssembly);
            foreach (string assemblyName in ThreadConfiguration.RequiredAssemblies)
            {
                host.LoadAssembly(assemblyName);
            }
            host.CreateWorker(ThreadConfiguration.ThreadStartTypeAssembly, ThreadConfiguration.ThreadStartTypeName);
            CrossDomainProxy = host;
            host.Start();
        }

        public void Pause()
        {
            CrossDomainProxy.Pause();
        }

        public void Resume()
        {
            CrossDomainProxy.Resume();
        }

        public void Stop()
        {
            CrossDomainProxy.Stop();
        }
    }
}
