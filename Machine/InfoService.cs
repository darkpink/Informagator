using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Net;
using Informagator.Contracts;
using Informagator.Contracts.Services;

namespace Informagator.Machine
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Single, InstanceContextMode=InstanceContextMode.Single, UseSynchronizationContext=true)]
    internal class InfoService : IInfoService
    {
        protected DefaultMachine Informagator { get; set; }
        public InfoService(DefaultMachine informagator)
        {
            Informagator = informagator;
        }
        public void Ping()
        {
            return;
        }

        public IThreadStatus GetStatus(string threadName)
        {
            return Informagator.Threads[threadName].Status;
        }
    }
}
