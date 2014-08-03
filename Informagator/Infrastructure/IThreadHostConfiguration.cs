using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IThreadHostConfiguration : IWorkerConfiguration
    {
        string WorkerClassTypeAssembly { get; }
        string WorkerClassTypeName { get; }
        bool IsSameAs(IThreadHostConfiguration config);
    }
}
