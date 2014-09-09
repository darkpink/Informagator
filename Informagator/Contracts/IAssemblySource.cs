using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    [ServiceContract]
    public interface IAssemblySource
     {
        [OperationContract]
        byte[] GetAssemblyBinary(string assemblyName);

        [OperationContract]
        byte[] GetDebuggingSymbolBinary(string assemblyName);
    }
}
