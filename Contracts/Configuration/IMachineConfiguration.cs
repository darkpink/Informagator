using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Configuration
{
    public interface IMachineConfiguration
    {
        string HostName { get; }

        string IPAddress { get; }

        int AdminServicePort { get; }
        
        string AdminServiceGroup { get; }
        
        int InfoServicePort { get; }
        
        string InfoServiceGroup { get; }
        
        IDictionary<string, IThreadConfiguration> ThreadConfiguration { get; }
    }
}
