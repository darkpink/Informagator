using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.WorkerServices
{
    public interface IMessageTracker
    {
        void TrackOutputMessage(IMessageTrackingInfo info, IMessage outputMessage);
    }
}
