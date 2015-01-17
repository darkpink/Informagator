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
        string MachineName { set; }
        IMachineConfiguration Configuration { get; }
    }
}
