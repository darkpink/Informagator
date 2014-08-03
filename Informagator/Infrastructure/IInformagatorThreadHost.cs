using Acadian.Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IInformagatorThreadHost : IInformagatorRunner<IThreadHostConfiguration>
    {
        IAssemblySource AssemblySource { set; }
    }
}
