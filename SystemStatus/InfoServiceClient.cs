using Informagator.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.SystemStatus
{
    public class InfoServiceClient : IInfoService, IDisposable
    {
        protected bool IsDisposed { get; set; }
        protected ChannelFactory<IInfoService> ChannelFactory { get; set; }

        protected IInfoService Channel { get; set; }

        public InfoServiceClient(string url)
        {
            IsDisposed = false;
            WSHttpBinding binding = new WSHttpBinding();
            ChannelFactory = new ChannelFactory<IInfoService>(binding, new EndpointAddress(url));
        }

        public void Ping()
        {
            EnsureChannel();

            try
            {
                Channel.Ping();
            }
            catch (Exception ex)
            {
                CloseChannel();
            }
        }

        public ThreadStatus GetStatus(string threadName)
        {
            ThreadStatus result;

            EnsureChannel();
            try
            {
                result = Channel.GetStatus(threadName);
            }
            catch (Exception)
            {
                CloseChannel();
                //TODO: build out a status object containing the error
                result = null;
            }

            return result;
        }

        private void CloseChannel()
        {
            if (Channel != null)
            {
                IClientChannel channel = (IClientChannel)Channel;
                try
                {
                    channel.Close();
                }
                catch
                {
                    try
                    {
                        channel.Abort();
                    }
                    catch { }
                }
                finally
                {
                    Channel = null;
                }
            }
        }

        protected void EnsureChannel()
        {
            if (Channel == null)
            {
                Channel = ChannelFactory.CreateChannel();
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                CloseChannel();
            }
        }
    }
}
