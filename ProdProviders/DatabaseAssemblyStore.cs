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

            using (AssemblyStoreEntities ent = new AssemblyStoreEntities())
            {
                Assembly asm = ent.Assemblies.Single(a => a.Name == assemblyName);
                assemblyBytes = asm.Executable;
            }

            return assemblyBytes;
        }

        public byte[] GetDebuggingSymbolBinary(string assemblyName)
        {
            //TODO - this is just going to return the assembly!
            byte[] debuggingSymbolBytes;

            using (AssemblyStoreEntities ent = new AssemblyStoreEntities())
            {
                Assembly asm = ent.Assemblies.Single(a => a.Name == assemblyName);
                debuggingSymbolBytes = asm.DebugSymbols;
            }

            return debuggingSymbolBytes;
        }
    }
}
