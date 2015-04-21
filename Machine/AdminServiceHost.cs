using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using Informagator.Contracts.Services;

namespace Informagator.Machine
{
    internal static class AdminServiceHost
    {
        private static ServiceHost adminServiceHost = null;

        internal static void StartService(IAdminService instance, int port)
        {
            adminServiceHost = new ServiceHost(instance);
            adminServiceHost.AddServiceEndpoint(typeof(IAdminService), new WSHttpBinding(), AdminServiceAddress.Format("localhost", port));
            adminServiceHost.Open();
        }

        internal static void StopService()
        {
            if (adminServiceHost != null)
            {
                if (adminServiceHost.State != CommunicationState.Closed)
                {
                    adminServiceHost.Close();
                }
            }
        }
    }
}
