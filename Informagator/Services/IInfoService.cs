using Informagator.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Services
{
    [ServiceContract]
    public interface IInfoService
    {
        [OperationContract]
        void Ping();

        [OperationContract]
        InformagatorThreadStatus GetStatus(string threadName);
    }
}
