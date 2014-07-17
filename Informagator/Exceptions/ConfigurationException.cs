using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Exceptions
{
    [Serializable]
    public class ConfigurationException : InformagatorException
    {
        public ConfigurationException()
            : base()
        {
        }

        public ConfigurationException(string message)
            : base(message)
        {
        }

        public ConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
