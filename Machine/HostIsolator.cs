using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Informagator.Contracts;
using System.Net;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.Exceptions;
using System.Diagnostics;

namespace Informagator.Machine
{
    public class HostIsolator : MarshalByRefObject
    {
        public  IWorkerConfiguration Configuration { get; protected set; }

        public string MachineName { get; protected set; }

        protected IsolatedWorkerHost CrossDomainProxy { get; set; }

        protected AppDomain InnerThreadDomain { get; set; }

        protected DateTime InitializedDateTime { get; set; }

        protected DateTime? StopDateTime { get; set; }

        protected Exception StatusException { get; set; }

        public HostIsolator(string machineName, IWorkerConfiguration configuration)
        {
            InitializedDateTime = DateTime.Now;
            MachineName = machineName;
            Configuration = configuration;
        }

        public void Start()
        {
            StatusException = null;

            try
            {
                InnerThreadDomain = AppDomain.CreateDomain(Configuration.Name);
                IsolatedWorkerHost host = InnerThreadDomain.CreateInstanceFromAndUnwrap(Assembly.GetExecutingAssembly().GetName().Name + ".dll", typeof(IsolatedWorkerHost).FullName, false, BindingFlags.CreateInstance, null, new object[] { MachineName, Configuration.Name }, null, null) as IsolatedWorkerHost;
                CrossDomainProxy = host;
                host.UnhandledException += InnerThreadDomain_UnhandledException;
                host.Start();
            }
            catch(Exception ex)
            {
                StatusException = ex;
                CrossDomainProxy = null; 
                
                if (InnerThreadDomain != null)
                {
                    try
                    {
                        AppDomain.Unload(InnerThreadDomain);
                    }
                    catch { }
                }

                InnerThreadDomain = null;
            }
        }

        protected void InnerThreadDomain_UnhandledException(Exception obj)
        {
            Informagator.Contracts.Exceptions.Action whatToDo;

            if (obj is InformagatorException)
            {
                whatToDo = ((InformagatorException)obj).SuggestedAction;
            }
            else
            {
                //seems like a reasonable default action
                //TODO: put a limit on the number of restarts within a timeframe, like windows services
                whatToDo = Contracts.Exceptions.Action.RestartThread;  
            }

            switch(whatToDo)
            {
                case Contracts.Exceptions.Action.RestartThread:
                    RestartAfterUnhandledException();
                    break;
                case Contracts.Exceptions.Action.TerminateApplication:
                    TerminateApplication();
                    break;
            }
        }

        protected void RestartAfterUnhandledException()
        {
            CrossDomainProxy = null;
            AppDomain.Unload(InnerThreadDomain);
            InnerThreadDomain = null;
            Start();
        }

        private void TerminateApplication()
        {
            //TODO: this should be attempted gracefully
            Process.GetCurrentProcess().Close();
        }

        public void Stop()
        {
            StopDateTime = DateTime.Now;
            if (CrossDomainProxy != null)
            {
                try
                {
                    CrossDomainProxy.Stop();
                }
                catch { }
            }

            if (InnerThreadDomain != null)
            {
                try
                {
                    AppDomain.Unload(InnerThreadDomain);
                }
                catch { }
            }

            InnerThreadDomain = null;
            CrossDomainProxy = null;
        }
        public IThreadStatus Status
        { 
            get
            {
                IThreadStatus result;
                if (CrossDomainProxy == null)
                {
                    //TODO this is wrong
                    result = new HostStatus();
                    result.HostName = Environment.MachineName;
                    result.ThreadName = Configuration.Name;
                    result.Stopped = StopDateTime;
                    result.Initialized = InitializedDateTime;
                    result.Info = result.Stopped == null ? "Not started" : "Stopped";
                    result.RunStatus = result.Stopped == null ? ThreadRunStatus.NotStarted : ThreadRunStatus.Stopped;

                    if (StatusException != null)
                    {
                        result.Info = StatusException.ToString();
                        result.RunStatus = ThreadRunStatus.Error;
                    }
                }
                else
                {
                    result = CrossDomainProxy.Status;
                    result.Initialized = InitializedDateTime;
                }
                
                return result;
            }
        }

        public bool AnyAssemblyChanged
        {
            get
            {
                bool result;

                if (CrossDomainProxy != null)
                {
                    result = CrossDomainProxy.AnyAssemblyChanged;
                }
                else
                {
                    result = true;
                }

                return result;
            }
        }

        public virtual bool IsRestartRequiredForNewConfiguration(IWorkerConfiguration newConfiguration)
        {
            bool result;

            if (CrossDomainProxy != null)
            {
                result = CrossDomainProxy.IsRestartRequiredForNewConfiguration(newConfiguration);
            }
            else
            {
                result = true;
            }

            return result;
        }
    }
}
