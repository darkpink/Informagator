using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager
{
    [Serializable]
    public class ExportedHostConfiguration : IMachineConfiguration
    {
        public string HostName { get; set; }

        public string IPAddress { get; set; }

        public IPAddress AdminServiceAddress { get; set; }

        public int AdminServicePort { get; set; }

        public string AdminServiceGroup { get; set; }

        public IPAddress InfoServiceAddress { get; set; }

        public int InfoServicePort { get; set; }

        public string InfoServiceGroup { get; set; }

        public IDictionary<string, IWorkerConfiguration> Workers { get; set; }

        public bool SuppressSystemConfigurationErrorHandlers { get; set; }
    }
}
