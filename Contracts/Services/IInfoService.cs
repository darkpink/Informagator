using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Services
{
    [ServiceContract]
    public interface IInfoService
    {
        [OperationContract]
        void Ping();

        [OperationContract]
        ThreadStatus GetStatus(string threadName);
    }
}
