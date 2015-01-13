using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Informagator.Contracts;
using Informagator.Configuration;
using System.Net;

namespace Informagator.Threads
{
    internal class Isolator
    {
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

        protected List<IPersistentService> PersistentServices { get; set; }
        protected AppDomain InnerThreadDomain { get; set; }
        public AssemblyManager AssemblySource { protected get; set; }
        public IMessageTracker MessageTracker { protected get; set; }
        public IMessageStore MessageStore { protected get; set; }
        public string Name { get; set; }

        protected DateTime InitializedDateTime { get; set; }
        protected DateTime? StopDateTime { get; set; }

        public Isolator()
        {
            InitializedDateTime = DateTime.Now;
            PersistentServices = new List<IPersistentService>();
        }

        public void Start()
        {
            InnerThreadDomain = AppDomain.CreateDomain(Configuration.Name);
            ThreadHost host = InnerThreadDomain.CreateInstanceFromAndUnwrap(Configuration.ThreadHostTypeAssembly, Configuration.ThreadHostTypeName,false, BindingFlags.CreateInstance, null, new object[] {Name}, null, null) as ThreadHost;
            CrossDomainProxy = host;
            StartPersistentServices();
            host.Start();
        }

        protected virtual void StartPersistentServices()
        {
            var currentSignatures = PersistentServices.Select(s => s.Signature);
            var desiredSignatures = CrossDomainProxy.PersistentServices.Select(s => s.Signature);

            //need to shut off any that we don't need first - they may be holding a resource that a new will will
            //need (like a server port or something)
            foreach(var svc in PersistentServices.Where(s => !desiredSignatures.Contains(s.Signature)).ToList())
            {
                svc.Stop();
                PersistentServices.Remove(svc);
            }

            foreach(IPersistentService svc in CrossDomainProxy.PersistentServices.Where(s => !currentSignatures.Contains(s.Signature)).ToList())
            {
                
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
        public InformagatorThreadStatus Status
        { 
            get
            {
                InformagatorThreadStatus result;
                if (CrossDomainProxy == null)
                {
                    result = new InformagatorThreadStatus();
                    result.HostName = Environment.MachineName;
                    result.ThreadName = Name;
                    result.IsRunning = false;
                    result.Stopped = StopDateTime;
                    result.Initialized = InitializedDateTime;
                    result.Info = result.Stopped == null ? "Stopped" : "Not started";
                }
                else
                {
                    result = CrossDomainProxy.Status;
                    result.Initialized = InitializedDateTime;
                }
                
                return result;
            }
        }
    }
}
