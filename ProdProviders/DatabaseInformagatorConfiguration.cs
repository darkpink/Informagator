using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.ProdProviders
{
    //TODO: get rid of this class and let the entity (detached) implement the interface
    public class DatabaseInformagatorConfiguration : IMachineConfiguration
    {
        private IDictionary<string, IThreadConfiguration> config { get; set; }

        public string IPAddress { get; protected set; }
        private string Host { get; set; }
        public DatabaseInformagatorConfiguration(string host, string ipAddress)
        {
            config = new Dictionary<string, IThreadConfiguration>();
            Host = host;
            IPAddress = ipAddress;
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

        public int AdminServicePort { get; set;}

        public string AdminServiceGroup { get; set;}

        public int InfoServicePort { get; set; }

        public string InfoServiceGroup { get; set;}
    }
}
