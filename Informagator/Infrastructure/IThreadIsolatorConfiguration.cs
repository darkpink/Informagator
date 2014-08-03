using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IThreadIsolatorConfiguration : IThreadHostConfiguration
    {
        string ThreadHostTypeAssembly { get; }
        string ThreadHostTypeName { get; }
        bool IsSameAs(IThreadIsolatorConfiguration config);

    }
}
