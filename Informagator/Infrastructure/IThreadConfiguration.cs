using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    public interface IThreadConfiguration
    {
        string Name { get; }
        string ThreadStartTypeAssembly { get; }
        string ThreadStartTypeName { get; }
        IList<string> RequiredAssemblies { get; set; }
        bool IsSameAs(IThreadConfiguration config);
    }
}
