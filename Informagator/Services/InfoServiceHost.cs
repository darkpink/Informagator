using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Acadian.Informagator.Services
{
    public static class InfoServiceHost
    {
        private static ServiceHost infoServiceHost = null;

        internal static void StartService(InfoService instance)
        {
            infoServiceHost = new ServiceHost(instance);
            infoServiceHost.AddServiceEndpoint(typeof(IInfoService), new NetNamedPipeBinding(), "net.pipe://localhost/InfoService");
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
