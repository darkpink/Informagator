using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acadian.Informagator.CommonComponents.ConsumerStages;
using Acadian.Informagator.Messages;
using Acadian.Informagator.CommonComponents.SupplierStages;
using Acadian.Informagator.Infrastructure;

namespace Tests
{
    [TestClass]
    public class Msmq
    {
        [TestMethod]
        public void TestConsumer()
        {
            TransactionalMsmqBinaryConsumer consumer = new TransactionalMsmqBinaryConsumer();
            consumer.QueueName = @".\private$\TestQueue";
            ByteArrayMessage msg = new ByteArrayMessage();
            msg.Body = new[] { (byte)65, (byte)65, (byte)65, (byte)65 };
            consumer.ConsumeMessage(msg);
        }

        [TestMethod]
        public void TestSupplier()
        {
            TransactionalMsmqBinarySupplier supplier = new TransactionalMsmqBinarySupplier();
            supplier.QueueName = @".\private$\TestQueue";
            IMessage msg = supplier.GetMessage();
        }

    }
}
