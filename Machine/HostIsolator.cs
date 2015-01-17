﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Informagator.Contracts;
using System.Net;
using Informagator.Contracts.Configuration;
using Informagator.Contracts.PersistentServices;
using Informagator.Contracts.Exceptions;
using System.Diagnostics;

namespace Informagator.Machine
{
    public class HostIsolator
    {
        public  IThreadConfiguration Configuration { get; set; }
 
        protected IsolatedWorkerHost CrossDomainProxy { get; set; }

        protected List<IPersistentService> PersistentServices { get; set; }

        protected AppDomain InnerThreadDomain { get; set; }

        protected DateTime InitializedDateTime { get; set; }

        protected DateTime? StopDateTime { get; set; }

        protected Exception StatusException { get; set; }

        public HostIsolator()
        {
            InitializedDateTime = DateTime.Now;
            PersistentServices = new List<IPersistentService>();
        }

        public void Start()
        {
            StatusException = null;

            try
            {
                InnerThreadDomain = AppDomain.CreateDomain(Configuration.Name);
                IsolatedWorkerHost host = InnerThreadDomain.CreateInstanceFromAndUnwrap(Configuration.ThreadHostTypeAssembly, Configuration.ThreadHostTypeName, false, BindingFlags.CreateInstance, null, new object[] { Configuration.Name }, null, null) as IsolatedWorkerHost;
                CrossDomainProxy = host;
                host.UnhandledException += InnerThreadDomain_UnhandledException;
                StartPersistentServices();
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

        protected virtual void StartPersistentServices()
        {
            var currentSignatures = PersistentServices.Select(s => s.Signature);
            var desiredSignatures = CrossDomainProxy.RequiredPersistentServices;

            //need to shut off any that we don't need first - they may be holding a resource that a new will will
            //need (like a server port or something)
            foreach(var svc in PersistentServices.Where(s => !desiredSignatures.Contains(s.Signature)).ToList())
            {
                svc.Stop();
                PersistentServices.Remove(svc);
            }

            foreach(IPersistentService svc in CrossDomainProxy.RequiredPersistentServices.Where(s => !currentSignatures.Contains(s)).ToList())
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
                    result.IsRunning = false;
                    result.Stopped = StopDateTime;
                    result.Initialized = InitializedDateTime;
                    result.Info = result.Stopped == null ? "Stopped" : "Not started";

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
    }
}
