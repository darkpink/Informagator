using Acadian.Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    public interface IInformagatorRunner
    {
        void Start();
        void Pause();
        void Resume();
        void Stop();
        string Name { set; }
        IInformagatorThreadStatus Status { get; }
    }
}
