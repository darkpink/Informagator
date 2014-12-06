using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Acadian.Informagator.Services
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Single, InstanceContextMode=InstanceContextMode.Single, UseSynchronizationContext=true)]
    internal class AdminService : IAdminService
    {
        private InformagatorService Informagator { get; set; }
        public AdminService(InformagatorService informagator)
        {
            Informagator = informagator;
        }
        public void Ping()
        {
            return;
        }

        public void ApplyConfiguration()
        {
            Informagator.ReloadConfiguration();
        }

        public void StartThread(string threadName)
        {
            Informagator.StartThread(threadName);
        }

        public void StopThread(string threadName)
        {
            Informagator.StopThread(threadName);
        }
    }
}
