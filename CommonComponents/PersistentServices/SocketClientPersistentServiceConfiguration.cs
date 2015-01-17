using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.PersistentServices
{
    public class SocketClientPersistentServiceConfiguration : IPersistentServiceConfiguration
    {
        public IPAddress Address { get; set; }

        public int Port { get; set; }

        public string ConfigurationSignature
        {
            get { return Address.ToString() + ":" + Port.ToString(); }
        }
    }
}
