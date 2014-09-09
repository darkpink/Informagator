using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Services
{
    [ServiceContract]
    internal interface IInfoService
    {
        [OperationContract]
        void Ping();
    }
}
