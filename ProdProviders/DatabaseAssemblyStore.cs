﻿using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reflection = System.Reflection;
using System.IO;
using Informagator.ProdProviders.Configuration;
using Informagator.Contracts.Providers;

namespace Informagator.ProdProviders
{
    public class DatabaseAssemblyStore : IAssemblyProvider
    {
        public byte[] GetAssemblyBinary(string assemblyName)
        {
            byte[] assemblyBytes;

            using (ConfigurationEntities ent = new ConfigurationEntities())
            {
                assemblyBytes = ent.SystemConfigurations
                                  .Single(a => a.IsActive)
                                  .AssemblySystemConfigurations
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
                debuggingSymbolBytes = ent.SystemConfigurations
                                  .Single(a => a.IsActive)
                                  .AssemblySystemConfigurations
                                  .Single(aav => aav.AssemblyName == assemblyName && aav.AssemblyDotNetVersion == "1.0.0.0")
                                  .AssemblyVersion.DebuggingSymbols;
            }

            return debuggingSymbolBytes;
        }
    }
}
