using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IPersistentService
    {
        IPersistentServiceSignature Signature { get; }

        void Start();

        void Stop();

        //suspend and resume are only used during a config reload.  expected behavior is that the service will
        //temporarily (*very* temporarily) withhold and communications it's doing with it's worker.
        void Suspend();

        void Resume();
    }

    public interface IPersistentService<TConfig> : IPersistentService
        where TConfig : IPersistentServiceConfiguration
    {
        TConfig Configuration { set; }
    }
}
