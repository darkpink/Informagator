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
        private List<Tuple<IMessage, IMessageTrackingInfo>> TrackingLog { get; set; }

        public MemoryMessageTracker()
        {
            TrackingLog = new List<Tuple<IMessage, IMessageTrackingInfo>>();
        }
        public void TrackOutputMessage(IMessageTrackingInfo info, IMessage message)
        {
            TrackingLog.Add(new Tuple<IMessage, IMessageTrackingInfo>(message, info));
            if (TrackingLog.Count > LogCount)
            {
                TrackingLog.RemoveAt(0);
            }
        }
    }
}
