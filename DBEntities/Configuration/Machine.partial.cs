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
        public string HostName
        {
            get 
            { 
                return Name; 
            }
        }

        IDictionary<string, IWorkerConfiguration> IMachineConfiguration.Workers
        {
            get 
            {
                return Workers == null ? null : Workers.ToDictionary(w => w.Name, w => (IWorkerConfiguration)w);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
