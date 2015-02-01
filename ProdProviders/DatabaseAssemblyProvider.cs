using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reflection = System.Reflection;
using System.IO;
using Informagator.Contracts.Providers;
using Informagator.DBEntities.Configuration;

namespace Informagator.ProdProviders
{
    public class DatabaseAssemblyProvider : IAssemblyProvider
    {
        public byte[] GetAssemblyBinary(string assemblyName, string assemblyVersion)
        {
            byte[] assemblyBytes;

            using (ConfigurationEntities ent = new ConfigurationEntities())
            {
                assemblyBytes = ent.SystemConfigurations
                                  .Single(a => a.IsActive)
                                  .Assemblies
                                  .Single(aav => aav.Name == assemblyName && aav.Version == assemblyVersion)
                                  .Executable;
            }

            return assemblyBytes;
        }

        public byte[] GetDebuggingSymbolBinary(string assemblyName, string assemblyVersion)
        {
            byte[] debuggingSymbolBytes;

            using (ConfigurationEntities ent = new ConfigurationEntities())
            {
                debuggingSymbolBytes = ent.SystemConfigurations
                                  .Single(a => a.IsActive)
                                  .Assemblies
                                  .Single(aav => aav.Name == assemblyName && aav.Version == assemblyVersion)
                                  .DebuggingSymbols;
            }

            return debuggingSymbolBytes;
        }
    }
}
