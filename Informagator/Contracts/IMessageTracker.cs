using Acadian.Informagator.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    [ServiceContract]

    public interface IMessageTracker
    {
        [OperationContract]
        void TrackMessage(TrackingInfo info, IMessage message);
    }
}
