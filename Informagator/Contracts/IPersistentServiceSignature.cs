using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IPersistentServiceSignature
    {
        string AssemblyName { get; set; }

        string AssemblyVersion { get; set; }

        //the signature is basically a hash of the configuration.  This is how the isolator will determine if
        //it already has the requested service running for the isolator
        string ConfigurationSignature { get; }

    }
}
