using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Configuration
{
    public class InformagatorConfiguration : IInformagatorConfiguration
    {
        private Dictionary<string, ThreadConfiguration> config { get; set; }
        private string Host { get; set; }
        public InformagatorConfiguration(string host)
        {
            config = new Dictionary<string, ThreadConfiguration>();
            Host = host;
        }

        public string HostName
        {
            get 
            {
                return "Informagator on " + Host;
            }
        }

        public Dictionary<string, ThreadConfiguration> ThreadConfiguration
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
