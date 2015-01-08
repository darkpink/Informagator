using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Threads
{
    internal class AssemblyManager
    {
        private Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();

        private IAssemblySource AssemblySource { get; set; }
        
        public AssemblyManager(IAssemblySource assemblySource)
        {
            AssemblySource = assemblySource;
        }

        public Assembly GetAssembly(string name)
        {
            Assembly result;

            Assembly[] ha = AppDomain.CurrentDomain.GetAssemblies();

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
                byte[] assemblyBytes = AssemblySource.GetAssemblyBinary(name);
                byte[] debuggingSymbolBytes = AssemblySource.GetDebuggingSymbolBinary(name);

                result = Assembly.Load(assemblyBytes, debuggingSymbolBytes);
                LoadedAssemblies.Add(name, result);
            }

            return result;
        }
    }
}
