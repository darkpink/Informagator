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
        public IInformagatorThreadStatus Status { get; protected set; }
        private IThreadIsolatorConfiguration _configuration;
        public  IThreadIsolatorConfiguration Configuration
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
        protected IInformagatorThreadHost CrossDomainProxy { get; set; }
       
        protected AppDomain InnerThreadDomain { get; set; }
        public IAssemblySource AssemblySource { protected get; set; }
        public IMessageTracker MessageTracker { protected get; set; }
        public IMessageStore MessageStore { protected get; set; }

        public string Name { get; set; }

        public void Start()
        {
            InnerThreadDomain = AppDomain.CreateDomain(Configuration.Name);
            string x = InnerThreadDomain.BaseDirectory;
            IInformagatorThreadHost host = InnerThreadDomain.CreateInstanceFromAndUnwrap(Configuration.ThreadHostTypeAssembly, Configuration.ThreadHostTypeName) as IInformagatorThreadHost;
            host.Configuration = Configuration;
            host.AssemblySource = AssemblySource;
            host.MessageStore = MessageStore;
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
