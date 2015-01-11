using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IServerSocketWorker
    {
        IPAddress Address { get; }
        int Port { get; }

    }
}
