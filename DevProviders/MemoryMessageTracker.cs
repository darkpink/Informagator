using Informagator.Contracts;
using Informagator.Contracts.WorkerServices;
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
        private List<Tuple<IMessage, ITrackingInfo>> TrackingLog { get; set; }

        public MemoryMessageTracker()
        {
            TrackingLog = new List<Tuple<IMessage, ITrackingInfo>>();
        }
        public void TrackMessage(ITrackingInfo info, IMessage message)
        {
            TrackingLog.Add(new Tuple<IMessage, ITrackingInfo>(message, info));
            if (TrackingLog.Count > LogCount)
            {
                TrackingLog.RemoveAt(0);
            }
        }
    }
}
