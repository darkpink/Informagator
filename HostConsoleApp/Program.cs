using Acadian.Informagator;
using Acadian.Informagator.DevProviders;
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
            InformagatorService i = new InformagatorService(new HardCodedConfigurationProvider(),
                                                            new FileSystemAssemblySource(),
                                                            new MemoryMessageStore());
            i.Start();
            Console.ReadLine();
            i.ReloadConfiguration();
            Console.ReadLine();
            i.Stop();
            Console.ReadLine();
        }
    }
}
