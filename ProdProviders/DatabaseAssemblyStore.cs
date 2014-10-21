using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reflection = System.Reflection;
using System.IO;

namespace Acadian.Informagator.ProdProviders
{
    public class DatabaseAssemblyStore : IAssemblySource
    {
        public byte[] GetAssemblyBinary(string assemblyName)
        {
            byte[] assemblyBytes;

            using (ConfigurationEntities ent = new ConfigurationEntities())
            {
                assemblyBytes = ent.ApplicationVersions
                                  .Single(a => a.IsCurrent)
                                  .AssemblyApplicationVersions
                                  .Single(aav => aav.AssemblyName == assemblyName && aav.AssemblyDotNetVersion == "1.0.0.0")
                                  .AssemblyVersion.Executable;
            }

            return assemblyBytes;
        }

        public byte[] GetDebuggingSymbolBinary(string assemblyName)
        {
            byte[] debuggingSymbolBytes;

            using (ConfigurationEntities ent = new ConfigurationEntities())
            {
                debuggingSymbolBytes = ent.ApplicationVersions
                                  .Single(a => a.IsCurrent)
                                  .AssemblyApplicationVersions
                                  .Single(aav => aav.AssemblyName == assemblyName && aav.AssemblyDotNetVersion == "1.0.0.0")
                                  .AssemblyVersion.DebuggingSymbols;
            }

            return debuggingSymbolBytes;
        }
    }
}
