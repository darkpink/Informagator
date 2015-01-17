using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Exceptions
{
    public enum Action { GotoNextStage, GotoNextMessage, RestartThread, StopThread, TerminateApplication };
    
    [Serializable]
    public class InformagatorException : Exception
    {
        public virtual Action SuggestedAction
        {
            get
            {
                return Action.RestartThread;
            }
        }
        
        public InformagatorException()
            : base()
        {
        }

        public InformagatorException(string message)
            : base(message)
        {
        }

        public InformagatorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InformagatorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
