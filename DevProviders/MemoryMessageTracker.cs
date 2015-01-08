using Informagator.Contracts;
using Informagator.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders
{
    public class MemoryMessageTracker : IMessageTracker
    {
        private const int LogCount = 100;
        private List<Tuple<IMessage, TrackingInfo>> TrackingLog { get; set; }

        public MemoryMessageTracker()
        {
            TrackingLog = new List<Tuple<IMessage, TrackingInfo>>();
        }
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
