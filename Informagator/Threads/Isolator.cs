using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Acadian.Informagator.Contracts;
using Acadian.Informagator.Configuration;

namespace Acadian.Informagator.Threads
{
    internal class Isolator
    {
        public InformagatorThreadStatus Status { get; protected set; }
        private ThreadConfiguration _configuration;
        public  ThreadConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
            set
            {
                _configuration = value;
                Name = _configuration.Name;
            }
        }
        protected ThreadHost CrossDomainProxy { get; set; }
       
        protected AppDomain InnerThreadDomain { get; set; }
        public AssemblyManager AssemblySource { protected get; set; }
        public IMessageTracker MessageTracker { protected get; set; }
        public IMessageStore MessageStore { protected get; set; }

        public string Name { get; set; }

        public void Start()
        {
            InnerThreadDomain = AppDomain.CreateDomain(Configuration.Name);
            string x = InnerThreadDomain.BaseDirectory;
            ThreadHost host = InnerThreadDomain.CreateInstanceFromAndUnwrap(Configuration.ThreadHostTypeAssembly, Configuration.ThreadHostTypeName,false, BindingFlags.CreateInstance, null, new object[] {Name}, null, null) as ThreadHost;
            CrossDomainProxy = host;
            host.Start();
        }

        public void Pause()
        {
            if (CrossDomainProxy != null)
            {
                CrossDomainProxy.Pause();
            }

        }

        public void Resume()
        {
            if (CrossDomainProxy != null)
            {
                CrossDomainProxy.Resume();
            }
        }

        public void Stop()
        {
            if (CrossDomainProxy != null)
            {
                CrossDomainProxy.Stop();
                CrossDomainProxy = null;
            }
        }
    }
}
