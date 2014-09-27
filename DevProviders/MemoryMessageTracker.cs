using Acadian.Informagator.Contracts;
using Acadian.Informagator.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.DevProviders
{
    public class MemoryMessageTracker : IMessageTracker
    {
        private const int LogCount = 100;
        private List<Tuple<IMessage, TrackingInfo>> TrackingLog { get; set; }
        public void TrackMessage(TrackingInfo info, IMessage message)
        {
            TrackingLog.Add(new Tuple<IMessage, TrackingInfo>(message, info));
            if (TrackingLog.Count > LogCount)
            {
                TrackingLog.RemoveAt(0);
            }
        }
    }
}
