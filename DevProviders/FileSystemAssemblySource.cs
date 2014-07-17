using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.DevProviders
{
    [Serializable]
    public class FileSystemAssemblySource : IAssemblySource
    {
        private Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();
        public Assembly LoadAssembly(string name)
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
                byte[] assemblyBytes;
                byte[] debuggingSymbolBytes;

                using (FileStream stream = new FileStream(name, FileMode.Open, FileAccess.Read))
                {
                    assemblyBytes = new byte[stream.Length];
                    stream.Read(assemblyBytes, 0, assemblyBytes.Length);
                }

                using (FileStream stream = new FileStream(name.Replace(".dll", ".pdb"), FileMode.Open, FileAccess.Read))
                {
                    debuggingSymbolBytes = new byte[stream.Length];
                    stream.Read(debuggingSymbolBytes, 0, debuggingSymbolBytes.Length);
                }

                result = Assembly.Load(assemblyBytes, debuggingSymbolBytes);
                LoadedAssemblies.Add(name, result);
            }
            
            return result;
        }
    }
}
