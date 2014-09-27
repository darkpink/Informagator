using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    public interface IMessage
    {
        Guid Id { get; }
        
        byte[] BinaryData { get; set; }
        
        IDictionary<string, string> Attributes { get; }
        
        IList<string> ProcessingTrail { get; }
    }

    public interface IMessage<TBody> : IMessage
    {
        TBody Body { get; set; }
    }
}
