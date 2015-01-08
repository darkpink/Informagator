using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Exceptions
{
    ///message exceptions indicate problems with the message - replaying the same message would always
    ///trigger the same message exception.  So the error should be logged and move on to the next message
    public class MessageException : InformagatorException
    {
        public MessageException()
            : base()
        {
        }

        public MessageException(string message)
            : base(message)
        {
        }

        public MessageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MessageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
