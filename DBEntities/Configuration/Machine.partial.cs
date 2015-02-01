using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DBEntities.Configuration
{
    public partial class Machine : IMachineConfiguration
    {
        //TODO protect against nullreferenceexceptions when not loaded
        public string HostName
        {
            get 
            { 
                return Name; 
            }
        }

        public string AdminServiceGroup
        {
            get 
            { 
                //TODO
                return null; 
            }
        }

        public string InfoServiceGroup
        {
            get 
            { 
                //TODO
                return null;
            }
        }

        IDictionary<string, IWorkerConfiguration> IMachineConfiguration.Workers
        {
            get 
            {
                return Workers.ToDictionary(w => w.Name, w => (IWorkerConfiguration)w);
            }
        }
    }
}
