using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    public class InformagatorConfiguration : IInformagatorConfiguration
    {
        private Dictionary<string, IThreadIsolatorConfiguration> config { get; set; }
        private string Host { get; set; }
        public InformagatorConfiguration(string host)
        {
            config = new Dictionary<string, IThreadIsolatorConfiguration>();
            Host = host;
        }

        public string ProcessName
        {
            get 
            {
                return "Informagator on " + Host;
            }
        }

        public IDictionary<string, IThreadIsolatorConfiguration> ThreadConfiguration
        {
            get 
            { 
                return config;
            }
        }

        public System.Net.IPAddress AdminServiceAddress { get; set;}

        public int AdminServicePort { get; set;}

        public string AdminServiceGroup { get; set;}

        public System.Net.IPAddress InfoServiceAddress { get; set;}

        public int InfoServicePort { get; set; }

        public string InfoServiceGroup { get; set;}
    }
}
