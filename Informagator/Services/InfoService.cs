using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Acadian.Informagator.Threads;
using System.Net;

namespace Acadian.Informagator.Services
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Single, InstanceContextMode=InstanceContextMode.Single, UseSynchronizationContext=true)]
    internal class InfoService : IInfoService
    {
        protected Informagator Informagator { get; set; }
        public InfoService(Informagator informagator)
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
