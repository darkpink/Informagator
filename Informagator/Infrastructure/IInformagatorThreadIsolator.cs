using Acadian.Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IInformagatorThreadIsolator
    {
        IAssemblySource AssemblySource { set; }
        IThreadConfiguration ThreadConfiguration { set; }
        void Start();
        void Pause();
        void Resume();
        void Stop();
    }
}
