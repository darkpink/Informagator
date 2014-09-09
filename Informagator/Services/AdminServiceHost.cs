using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Acadian.Informagator.Services
{
    internal static class AdminServiceHost
    {
        private static ServiceHost adminServiceHost = null;

        internal static void StartService(AdminService instance)
        {
            adminServiceHost = new ServiceHost(instance);
            adminServiceHost.AddServiceEndpoint(typeof(IAdminService), new NetNamedPipeBinding(), "net.pipe://localhost/AdminService");
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
