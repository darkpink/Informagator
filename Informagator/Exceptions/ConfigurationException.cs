using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Exceptions
{
    /// <summary>
    /// Self explanatory: ConfigurationExceptions cause the thread to terminate due to a non-code, non-message
    /// issue.  A good example would be a parameter for a directory that doesn't exist
    /// </summary>
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
