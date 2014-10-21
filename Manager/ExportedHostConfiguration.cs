using Acadian.Informagator.Configuration;
using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Manager
{
    [Serializable]
    public class ExportedHostConfiguration : IInformagatorConfiguration
    {
        public string HostName { get; set; }

        public IPAddress AdminServiceAddress { get; set; }

        public int AdminServicePort { get; set; }

        public string AdminServiceGroup { get; set; }

        public IPAddress InfoServiceAddress { get; set; }

        public int InfoServicePort { get; set; }

        public string InfoServiceGroup { get; set; }

        public Dictionary<string, ThreadConfiguration> ThreadConfiguration { get; set; }

    }
}
