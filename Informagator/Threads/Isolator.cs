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
        public InformagatorThreadStatus Status { get; protected set; }
        public  IThreadConfiguration ThreadConfiguration { protected get; set; }

        protected IInformagatorThreadHost CrossDomainProxy { get; set; }
        
        protected AppDomain InnerThreadDomain { get; set; }

        public IAssemblySource AssemblySource { protected get; set; }

        public void Start()
        {
            InnerThreadDomain = AppDomain.CreateDomain(ThreadConfiguration.Name);
            IInformagatorThreadHost host = InnerThreadDomain.CreateInstanceAndUnwrap(ThreadConfiguration.ThreadHostTypeAssembly, ThreadConfiguration.ThreadHostTypeName) as IInformagatorThreadHost;
            host.Configuration = ThreadConfiguration;
            host.AssemblySource = AssemblySource;
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
