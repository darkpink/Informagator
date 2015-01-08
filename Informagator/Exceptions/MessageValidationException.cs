using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Exceptions
{
    public class MessageValidationException : MessageException
    {
        public MessageValidationException()
            : base()
        {
        }

        public MessageValidationException(string message)
            : base(message)
        {
        }

        public MessageValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MessageValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
