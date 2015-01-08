using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Informagator.Threads;
using System.Net;

namespace Informagator.Services
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Single, InstanceContextMode=InstanceContextMode.Single, UseSynchronizationContext=true)]
    internal class InfoService : IInfoService
    {
        protected Machine Informagator { get; set; }
        public InfoService(Machine informagator)
        {
            Informagator = informagator;
        }
        public void Ping()
        {
            return;
        }

        public InformagatorThreadStatus GetStatus(string threadName)
        {
            return Informagator.Threads[threadName].Status;
        }
    }
}
