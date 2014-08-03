using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IInformagatorRunner<TConfig>
    {
        void Start();
        void Pause();
        void Resume();
        void Stop();
        string Name { set; }
        IInformagatorThreadStatus Status { get; }
        IMessageStore MessageStore { set; }
        IMessageTracker MessageTracker { set; }
        TConfig Configuration { get; set; }
    }
}
