using Acadian.Informagator.Configuration;
using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = true)]
    internal class InternalService : IInternalService, IAssemblySource
    {
        internal const string Endpoint = "net.pipe://localhost/asdf/InternalService";
        private IMessageStore MessageStore { get; set; }
        private IAssemblySource AssemblySource { get; set; }
        private IMessageTracker MessageTracker { get; set; }

        private IInformagatorConfiguration Configuration { get; set; }

        public InternalService(IMessageStore messageStore, IMessageTracker messageTracker, IAssemblySource assemblySource, IConfigurationProvider configurationProvider)
        {
            MessageStore = messageStore;
            MessageTracker = messageTracker;
            AssemblySource = assemblySource;
            Configuration = configurationProvider.Configuration;
        }

        public void Enqueue(string queueName, SerializedMessage message)
        {
            MessageStore.Enqueue(queueName, message);
        }

        public SerializedMessage Dequeue(string queueName)
        {
            SerializedMessage result = null;

            IMessage message = MessageStore.Dequeue(queueName);

            if (message != null)
            {
                result = new SerializedMessage(message);
            }
            
            return result;

        }

        public void TrackMessage(Tracking.TrackingInfo info, SerializedMessage message)
        {
            MessageTracker.TrackMessage(info, message);
        }


        public byte[] GetAssemblyBinary(string assemblyName)
        {
            return AssemblySource.GetAssemblyBinary(assemblyName);
        }

        public byte[] GetDebuggingSymbolBinary(string assemblyName)
        {
            return AssemblySource.GetDebuggingSymbolBinary(assemblyName);
        }


        public ThreadConfiguration GetThreadHostConfiguration(string name)
        {
            return Configuration.ThreadConfiguration[name];
        }
    }
}
