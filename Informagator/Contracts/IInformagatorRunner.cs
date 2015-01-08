using Informagator.Configuration;
using Informagator.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IInformagatorRunner
    {
        void Start();
        void Stop();
        string Name { set; }
        InformagatorThreadStatus Status { get; }
    }
}
