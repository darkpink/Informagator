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
    internal static class InfoServiceHost
    {
        private static ServiceHost infoServiceHost = null;

        internal static void StartService(IInfoService instance, int port)
        {
            infoServiceHost = new ServiceHost(instance);
            infoServiceHost.AddServiceEndpoint(typeof(IInfoService), new WSHttpBinding(), InfoServiceAddress.Format("localhost", port));
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
