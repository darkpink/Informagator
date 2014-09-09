using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Acadian.Informagator.Services
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Single, InstanceContextMode=InstanceContextMode.Single, UseSynchronizationContext=true)]
    internal class InfoService : IInfoService
    {
        public void Ping()
        {
            return;
        }
    }
}
