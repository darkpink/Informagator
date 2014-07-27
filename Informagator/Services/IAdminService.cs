using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Services
{
    [ServiceContract]
    internal interface IAdminService
    {
        [OperationContract]
        void Ping();

        [OperationContract]
        void ApplyConfiguration();

        [OperationContract]
        void StartThread(string threadName);

        [OperationContract]
        void StopThread(string threadName);

        [OperationContract]
        void PauseThread(string threadName);

        [OperationContract]
        void ResumeThread(string threadName);
    }
}
