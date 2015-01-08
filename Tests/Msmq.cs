using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Informagator.CommonComponents.ConsumerStages;
using Informagator.Messages;
using Informagator.CommonComponents.SupplierStages;
using Informagator.Contracts;

namespace Tests
{
    [TestClass]
    public class Msmq
    {
        [TestMethod]
        public void TestConsumer()
        {
            StaticTransactionalMsmqBinaryConsumer consumer = new StaticTransactionalMsmqBinaryConsumer();
            consumer.QueueName = @".\private$\TestQueue";
            ByteArrayMessage msg = new ByteArrayMessage();
            msg.Body = new[] { (byte)65, (byte)65, (byte)65, (byte)65 };
            consumer.Consume(msg);
        }

        [TestMethod]
        public void TestSupplier()
        {
            TransactionalMsmqBinarySupplier supplier = new TransactionalMsmqBinarySupplier();
            supplier.QueueName = @".\private$\TestQueue";
            IMessage msg = supplier.Supply();
        }

    }
}
