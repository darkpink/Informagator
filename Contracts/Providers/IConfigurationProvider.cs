using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Providers
{
    public interface IConfigurationProvider
    {
        IEnumerable<string> GetActiveMachineNames();
        
        IMachineConfiguration GetMachineConfiguration(string machineName);

        IWorkerConfiguration GetThreadConfiguration(string machineName, string threadName);
    }
}
