using Informagator.CommonComponents.ErrorHandlers;
using Informagator.CommonComponents.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.CommonComponents.ErrorHandlers
{
    [TestClass]
    public class EventLogErrorHandlerTests
    {
        [TestMethod]
        public void HandlerLogsCorrectly()
        {
            EventLogErrorHandler handler = new EventLogErrorHandler();
            handler.Source = "InformagatorTests";
            handler.Handle(new[] {"Test"}, new InvalidOperationException(), new AsciiStringMessage() { Body = "test" });
        }
    }
}
