﻿using Acadian.Informagator.Configuration;
using Acadian.Informagator.Contracts;
using Acadian.Informagator.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Services
{
    [ServiceContract]
    internal interface IInternalService //: IAssemblySource, IMessageStore, IMessageTracker, IConfigurationSource
    {
        [OperationContract]
        void Enqueue(string queueName, SerializedMessage message);

        [OperationContract]
        SerializedMessage Dequeue(string queueName);

        [OperationContract]
        void TrackMessage(TrackingInfo info, SerializedMessage message);

        [OperationContract]
        byte[] GetAssemblyBinary(string assemblyName);

        [OperationContract]
        byte[] GetDebuggingSymbolBinary(string assemblyName);

        [OperationContract]
        ThreadConfiguration GetThreadHostConfiguration(string name);
    }
}