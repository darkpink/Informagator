using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Exceptions
{
    /// <summary>
    /// These exceptions are to be thrown when a code path that isn't suppsed to be reached is hit.  They
    /// indicate a problem in code.  TODO: decide if I want a property in this exception called IsFatal
    /// which would cause the thread to gracefully terminate, or should I create another exception type
    /// </summary>
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
