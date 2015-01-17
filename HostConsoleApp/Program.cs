using Informagator.Contracts;
using Informagator.DevProviders;
using Informagator.Machine;
using Informagator.ProdProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HostConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryMessageStore store = new MemoryMessageStore();

            string machineNameOverride = args.Length > 0 ? args[0] : null;

            IMachine i = new DefaultMachine(machineNameOverride);
            i.Start();
            Console.ReadLine();
            //i.ReloadConfiguration();
            //Console.ReadLine();
            i.Stop();
            Console.ReadLine();
        }
    }
}
