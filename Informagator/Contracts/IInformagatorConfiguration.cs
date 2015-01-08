using Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IInformagatorConfiguration
    {
        string HostName { get; }
        IPAddress AdminServiceAddress { get; }
        int AdminServicePort { get; }
        string AdminServiceGroup { get; }
        IPAddress InfoServiceAddress { get; }
        int InfoServicePort { get; }
        string InfoServiceGroup { get; }
        Dictionary<string, ThreadConfiguration> ThreadConfiguration { get; }
    }
}
