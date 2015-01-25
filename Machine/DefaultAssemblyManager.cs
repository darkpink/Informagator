using Informagator.Contracts;
using Informagator.Contracts.Providers;
using Informagator.Contracts.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Machine
{
    public class DefaultAssemblyManager : IAssemblyManager
    {
        private Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();
        private Dictionary<string, byte[]> LoadedAssemblyBytes = new Dictionary<string, byte[]>();

        private IAssemblyProvider AssemblyProvider { get; set; }
        
        public DefaultAssemblyManager(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        protected virtual Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //TODO: make sure this is going to work
            return GetAssembly(args.Name);
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
                byte[] assemblyBytes = AssemblyProvider.GetAssemblyBinary(name);
                byte[] debuggingSymbolBytes = AssemblyProvider.GetDebuggingSymbolBinary(name);

                result = Assembly.Load(assemblyBytes, debuggingSymbolBytes);
                LoadedAssemblies.Add(name, result);
                LoadedAssemblyBytes.Add(name, assemblyBytes);
            }

            return result;
        }


        public bool AnyAssemblyChanged
        {
            get 
            {
                bool result = false;

                foreach(string assemblyName in LoadedAssemblies.Keys)
                {
                    byte[] existingAssemblyBytes = LoadedAssemblyBytes[assemblyName];
                    byte[] newAssemblyBytes = AssemblyProvider.GetAssemblyBinary(assemblyName);
                    if (!existingAssemblyBytes.SequenceEqual(newAssemblyBytes))
                    {
                        result = true;
                        break;
                    }
                }

                return result;
            }
        }
    }
}
