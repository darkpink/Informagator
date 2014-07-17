using Acadian.Informagator.Infrastructure;
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
        private Dictionary<string, Reflection.Assembly> LoadedAssemblies = new Dictionary<string, Reflection.Assembly>();
        public Reflection.Assembly LoadAssembly(string name)
        {
            Reflection.Assembly result;

            Reflection.Assembly[] ha = AppDomain.CurrentDomain.GetAssemblies();

            if (LoadedAssemblies.ContainsKey(name))
            {
                result = LoadedAssemblies[name];
            }
            else if (AppDomain.CurrentDomain.GetAssemblies().Any(a => a.ManifestModule.ScopeName == name))
            {
                result = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.ManifestModule.ScopeName == name);
            }
            else
            {
                byte[] assemblyBytes;
                byte[] debuggingSymbolBytes;

                using (AssemblyStoreEntities ent = new AssemblyStoreEntities())
                {
                    Assembly asm = ent.Assemblies.Single(a => a.Name == name);
                    assemblyBytes = asm.Executable;
                    debuggingSymbolBytes = asm.DebugSymbols;
                }

                result = Reflection.Assembly.Load(assemblyBytes, debuggingSymbolBytes);
                LoadedAssemblies.Add(name, result);
            }

            return result;
        }
    }
}
