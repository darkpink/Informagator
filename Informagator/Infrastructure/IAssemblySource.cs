using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IAssemblySource
    {
        Assembly LoadAssembly(string assemblyName);
    }
}
