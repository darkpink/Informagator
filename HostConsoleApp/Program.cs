using Acadian.Informagator;
using Acadian.Informagator.DevProviders;
using Acadian.Informagator.Messages;
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

            InformagatorService i = new InformagatorService(new HardCodedConfigurationProvider(),
                                                            new FileSystemAssemblySource(),
                                                            store);
            i.Start();
            Console.ReadLine();
            i.ReloadConfiguration();
            Console.ReadLine();
            i.Stop();
            Console.ReadLine();
        }
    }
}
