﻿using Informagator.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.SystemStatus
{
    public class InfoServiceClient : IInfoService
    {
        protected ChannelFactory<IInfoService> ChannelFactory { get; set; }

        public InfoServiceClient(string url)
        {
            WSHttpBinding binding = new WSHttpBinding();
            ChannelFactory = new ChannelFactory<IInfoService>(binding, new EndpointAddress(url));
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

        public ThreadStatus GetStatus(string threadName)
        {
            ThreadStatus result;

            var client = ChannelFactory.CreateChannel();
            try
            {
                result = client.GetStatus(threadName);
                ((IClientChannel)client).Close();
            }
            catch (Exception)
            {
                ((IClientChannel)client).Abort();
                result = null;
            }

            return result;
        }
    }
}
