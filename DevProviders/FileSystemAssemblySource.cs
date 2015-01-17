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
        public byte[] GetAssemblyBinary(string assemblyName)
        {
            byte[] assemblyBytes;

            using (FileStream stream = new FileStream(assemblyName, FileMode.Open, FileAccess.Read))
            {
                assemblyBytes = new byte[stream.Length];
                stream.Read(assemblyBytes, 0, assemblyBytes.Length);
            }


            return assemblyBytes;
        }

        public byte[] GetDebuggingSymbolBinary(string assemblyName)
        {
            byte[] debuggingSymbolBytes;

            using (FileStream stream = new FileStream(assemblyName.Replace(".dll", ".pdb"), FileMode.Open, FileAccess.Read))
            {
                debuggingSymbolBytes = new byte[stream.Length];
                stream.Read(debuggingSymbolBytes, 0, debuggingSymbolBytes.Length);
            }

            return debuggingSymbolBytes;
        }
    }
}
