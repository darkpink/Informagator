using Informagator.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager
{
    class AdminServiceClient
    {
        protected ChannelFactory<IAdminService> ChannelFactory { get; set; }

        public AdminServiceClient(string url)
        {
            WSHttpBinding binding = new WSHttpBinding();
            ChannelFactory = new ChannelFactory<IAdminService>(binding, new EndpointAddress(url));
        }

        public void Ping()
        {
            var client = ChannelFactory.CreateChannel();
            try
            {
                client.Ping();
                ((IClientChannel)client).Close();
            }
            catch (Exception)
            {
                ((IClientChannel)client).Abort();
                throw;
            }
        }

        public void StartThread(string threadName)
        {

            var client = ChannelFactory.CreateChannel();
            try
            {
                client.StartThread(threadName);
                ((IClientChannel)client).Close();
            }
            catch (Exception)
            {
                ((IClientChannel)client).Abort();
            }
        }
        public void StopThread(string threadName)
        {

            var client = ChannelFactory.CreateChannel();
            try
            {
                client.StopThread(threadName);
                ((IClientChannel)client).Close();
            }
            catch (Exception)
            {
                ((IClientChannel)client).Abort();
            }
        }
    }
}
