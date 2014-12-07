using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;

namespace Acadian.Informagator.Services
{
    internal static class InfoServiceHost
    {
        private static ServiceHost infoServiceHost = null;

        internal static void StartService(InfoService instance, IPAddress address, int port)
        {
            infoServiceHost = new ServiceHost(instance);
            infoServiceHost.AddServiceEndpoint(typeof(IInfoService), new WSHttpBinding(), "http://localhost:" + port + "/InfoService");
            infoServiceHost.Open();
        }

        internal static void StopService()
        {
            if (infoServiceHost != null)
            {
                if (infoServiceHost.State != CommunicationState.Closed)
                {
                    infoServiceHost.Close();
                }
            }
        }
    }
}
