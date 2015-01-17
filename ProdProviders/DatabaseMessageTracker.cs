using Informagator.Contracts;
using Informagator.Contracts.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.ProdProviders
{
    public class DatabaseMessageTracker : IMessageTracker
    {
        public void TrackMessage(ITrackingInfo info, IMessage message)
        {
            
        }
    }
}
