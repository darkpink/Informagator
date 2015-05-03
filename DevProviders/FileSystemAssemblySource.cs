using Informagator.Contracts;
using Informagator.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders
{
    public class FileSystemAssemblySource : IAssemblyProvider
    {
        protected string AssemblyDirectory { get; set; }

        public FileSystemAssemblySource(string assemblyDirectory)
        {
            AssemblyDirectory = assemblyDirectory;
        }

        public byte[] GetAssemblyBinary(string assemblyName, string assemblyVersion)
        {
            byte[] assemblyBytes;

            string path = Path.Combine(AssemblyDirectory, assemblyName);

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                assemblyBytes = new byte[stream.Length];
                stream.Read(assemblyBytes, 0, assemblyBytes.Length);
            }


            return assemblyBytes;
        }

        public byte[] GetDebuggingSymbolBinary(string assemblyName, string assemblyVersion)
        {
            byte[] debuggingSymbolBytes;

            string path = Path.Combine(AssemblyDirectory, assemblyName).Replace(".dll", ".pdb");

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                debuggingSymbolBytes = new byte[stream.Length];
                stream.Read(debuggingSymbolBytes, 0, debuggingSymbolBytes.Length);
            }

            return debuggingSymbolBytes;
        }
    }
}
