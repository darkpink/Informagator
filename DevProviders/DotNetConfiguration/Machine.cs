using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class Machine : ConfigurationElement, IMachineConfiguration
    {
        [ConfigurationProperty("hostName", IsKey = true, IsRequired = true)]
        public string HostName
        {
            get
            {
                return (string)base["hostName"];
            }
        }

        [ConfigurationProperty("errorHandlers")]
        [ConfigurationCollection(typeof(ErrorHandlerCollection), AddItemName = "errorHandler")]
        public ErrorHandlerCollection ErrorHandlers
        {
            get
            {
                return (ErrorHandlerCollection)base["errorHandlers"];
            }
        }

        [ConfigurationProperty("workers")]
        [ConfigurationCollection(typeof(WorkerCollection), AddItemName = "worker")]
        public WorkerCollection Workers
        {
            get
            {
                return (WorkerCollection)base["workers"];
            }
        }

        [ConfigurationProperty("ipAddress")]
        public string IPAddress
        {
            get 
            { 
                return (string)base["ipAddress"]; 
            }
        }

        [ConfigurationProperty("adminServicePort")]
        public int AdminServicePort
        {
            get 
            { 
                return (int)base["adminServicePort"]; 
            }
        }

        [ConfigurationProperty("adminServiceGroup")]
        public string AdminServiceGroup
        {
            get 
            { 
                return (string)base["adminServiceGroup"]; 
            }
        }

        [ConfigurationProperty("infoServicePort")]
        public int InfoServicePort
        {
            get 
            { 
                return (int)base["infoServicePort"]; 
            }
        }

        [ConfigurationProperty("infoServiceGroup")]
        public string InfoServiceGroup
        {
            get 
            { 
                return (string)base["infoServiceGroup"]; 
            }
        }

        [ConfigurationProperty("suppressParentErrorHandlers")]
        public bool SuppressParentErrorHandlers
        {
            get 
            { 
                return (bool)base["suppressParentErrorHandlers"]; 
            }
        }

        IDictionary<string, IWorkerConfiguration> IMachineConfiguration.Workers
        {
            get 
            { 
                return Workers.OfType<IWorkerConfiguration>().ToDictionary(w => w.Name); 
            }
        }
    }
}
