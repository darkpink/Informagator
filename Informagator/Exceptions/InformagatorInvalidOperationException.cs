using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Exceptions
{
    [Serializable]
    public class InformagatorInvalidOperationException : InformagatorException
    {
        public InformagatorInvalidOperationException()
            : base()
        {
        }

        public InformagatorInvalidOperationException(string message)
            : base(message)
        {
        }

        public InformagatorInvalidOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InformagatorInvalidOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
