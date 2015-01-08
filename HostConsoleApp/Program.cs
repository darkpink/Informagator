using Informagator.DevProviders;
using Informagator.Messages;
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
            store.Enqueue("Demo", new AsciiStringMessage() { Body = "asdfasdf" });

            Informagator.Machine i = new Informagator.Machine(new DatabaseConfigurationProvider(),
                                                            new FileSystemAssemblySource(),
                                                            store,
                                                            new MemoryMessageTracker());
            i.Start();
            Console.ReadLine();
            //i.ReloadConfiguration();
            //Console.ReadLine();
            i.Stop();
            Console.ReadLine();
        }
    }
}
