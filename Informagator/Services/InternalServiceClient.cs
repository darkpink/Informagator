using Informagator.Configuration;
using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Services
{
    internal class InternalServiceClient : IInternalService, IAssemblySource, IMessageStore, IMessageTracker
    {
        protected IInternalService ServiceProxy { get; set; }

        protected ChannelFactory<IInternalService> InternalServiceChannelFactory { get; set; }

        public InternalServiceClient()
        {
            RebuildProxy();
        }

        public void Enqueue(string queueName, IMessage message)
        {
            Try(() => ServiceProxy.Enqueue(queueName, new SerializedMessage(message)));
        }

        public IMessage Dequeue(string queueName)
        {
            IMessage result;

            result = Try(() => ServiceProxy.Dequeue(queueName));

            return result;
        }

        public void TrackMessage(Tracking.TrackingInfo info, Contracts.IMessage message)
        {
            Try(() => ServiceProxy.TrackMessage(info, new SerializedMessage(message)));
        }

        public byte[] GetAssemblyBinary(string assemblyName)
        {
            byte[] result;

            result = Try(() => ServiceProxy.GetAssemblyBinary(assemblyName));
            
            return result;
        }

        public byte[] GetDebuggingSymbolBinary(string assemblyName)
        {
            byte[] result;

            result = Try(() => ServiceProxy.GetDebuggingSymbolBinary(assemblyName));
            
            return result;
        }
        protected T Try<T>(Func<T> toExecute)
        {
            T result;

            try
            {
                result = toExecute();
            }
            catch (CommunicationException)
            {
                RebuildProxy();
                throw;
            }
            
            return result;
        }

        protected void Try(Action toExecute)
        {
            try
            {
                toExecute();
            }
            catch (CommunicationException)
            {
                RebuildProxy();
                throw;
            }
        }

        protected void RebuildProxy()
        {
            if (InternalServiceChannelFactory != null)
            {
                try 
                { 
                    InternalServiceChannelFactory.Close();
                } 
                catch
                { 
                    InternalServiceChannelFactory.Abort();
                }
            }

            InternalServiceChannelFactory = new ChannelFactory<IInternalService>(
                new NetNamedPipeBinding(NetNamedPipeSecurityMode.None), 
                new EndpointAddress(InternalService.Endpoint));
            ServiceProxy = InternalServiceChannelFactory.CreateChannel();
            ((ICommunicationObject)ServiceProxy).Open();
        }


        public ThreadConfiguration GetThreadHostConfiguration(string name)
        {
            ThreadConfiguration result;

            result = Try(() => ServiceProxy.GetThreadHostConfiguration(name));
            
            return result;
        }

        void IInternalService.Enqueue(string queueName, SerializedMessage message)
        {
            Try(() => ServiceProxy.Enqueue(queueName, message));
        }

        SerializedMessage IInternalService.Dequeue(string queueName)
        {
            SerializedMessage result;

            result = Try(() => ServiceProxy.Dequeue(queueName));

            return result;
        }

        void IInternalService.TrackMessage(Tracking.TrackingInfo info, SerializedMessage message)
        {
            Try(() => ServiceProxy.TrackMessage(info, message));
        }
    }
}
