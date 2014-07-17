using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Exceptions
{
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
