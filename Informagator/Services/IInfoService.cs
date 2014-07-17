using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Services
{
    [ServiceContract]
    interface IInfoService
    {
        [OperationContract]
        void Ping();
    }
}
