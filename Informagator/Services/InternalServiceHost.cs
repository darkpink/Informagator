using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Services
{
    internal class InternalServiceHost
    {
        private static ServiceHost internalServiceHost = null;

        internal static void StartService(InternalService instance)
        {
            internalServiceHost = new ServiceHost(instance);
            internalServiceHost.AddServiceEndpoint(
                typeof(IInternalService),
                new NetNamedPipeBinding(NetNamedPipeSecurityMode.None),
                new Uri(InternalService.Endpoint));
            internalServiceHost.Open();
        }

        internal static void StopService()
        {
            if (internalServiceHost != null)
            {
                if (internalServiceHost.State != CommunicationState.Closed)
                {
                    internalServiceHost.Close();
                }
            }
        }
    }
}
