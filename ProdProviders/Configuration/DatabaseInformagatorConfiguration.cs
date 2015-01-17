using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.ProdProviders.Configuration
{
    //TODO: get rid of this class and let the entity (detached) implement the interface
    public class DatabaseInformagatorConfiguration : IMachineConfiguration
    {
        private IDictionary<string, IThreadConfiguration> config { get; set; }
        private string Host { get; set; }
        public DatabaseInformagatorConfiguration(string host)
        {
            config = new Dictionary<string, IThreadConfiguration>();
            Host = host;
        }

        public string HostName
        {
            get 
            {
                return "Informagator on " + Host;
            }
        }

        public IDictionary<string, IThreadConfiguration> ThreadConfiguration
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
