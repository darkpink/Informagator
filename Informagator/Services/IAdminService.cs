using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Services
{
    [ServiceContract]
    public interface IAdminService
    {
        [OperationContract]
        void Ping();

        [OperationContract]
        void ApplyConfiguration();

        [OperationContract]
        void StartThread(string threadName);

        [OperationContract]
        void StopThread(string threadName);
    }
}
