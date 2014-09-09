using Acadian.Informagator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    [ServiceContract]

    interface IConfigurationSource
    {
        [OperationContract]
        ThreadConfiguration GetThreadHostConfiguration(string name);
    }
}
