using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager
{
    public static class Utilities
    {
        public static string[] GetTypeNamesImplementingInterfaceFromAssembly(Type interfaceType, byte[] assemblyBinary)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.Load(assemblyBinary);

            var result = asm.GetTypes()
                        .Where(t => t.GetInterfaces().Any(i => i.FullName == interfaceType.FullName))
                        .Select(t => t.FullName)
                        .OrderBy(n => n)
                        .ToArray();
            
            return result;
        }
    }
}
