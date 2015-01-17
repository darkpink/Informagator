using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Exceptions
{
    [Serializable]
    public class ErrorHandlerException : InformagatorException
    {
        ///This is really, really bad.  This exception means that an error couldn't be logged/handled.  So we've
        ///screwed up processing a message and now we won't be able to journal what the issue was.
        ///Informagator behavior is TBD, but I'm thinking by default the worker should be terminated with a nasty error
        ///status.

        public ErrorHandlerException()
            : base()
        {
        }

        public ErrorHandlerException(string message)
            : base(message)
        {
        }

        public ErrorHandlerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ErrorHandlerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
