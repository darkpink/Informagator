using Acadian.Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IInfomagatorThreadHost
    {
        void Start();
        void Pause();
        void Resume();
        void Stop();
        //void LoadAssembly(string name, byte[] assembly);
        void CreateWorker(string assembly, string type);
        IInformagatorThreadStatus Status { get; }
        IThreadConfiguration Configuration { set; }
    }
}
