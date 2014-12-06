using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;

namespace Acadian.Informagator.Services
{
    internal static class AdminServiceHost
    {
        private static ServiceHost adminServiceHost = null;

        internal static void StartService(AdminService instance, IPAddress address, int port)
        {
            adminServiceHost = new ServiceHost(instance);
            adminServiceHost.AddServiceEndpoint(typeof(IAdminService), new WSHttpBinding(), "http://" + address.ToString() + ":" + port + "/AdminService");
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
