using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    public interface IInformagatorConfiguration
    {
        string ProcessName { get; }
        IPAddress AdminServiceAddress { get; }
        int AdminServicePort { get; }
        string AdminServiceGroup { get; }
        IPAddress InfoServiceAddress { get; }
        int InfoServicePort { get; }
        string InfoServiceGroup { get; }
        IDictionary<string, IThreadConfiguration> ThreadConfiguration { get; }
    }
}
