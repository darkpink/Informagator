using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IInformagatorWorkerClass
    {
        void Run();
        void Pause();
        void Resume();
        void Stop();
        IInformagatorThreadStatus Status { get; }
    }
}
