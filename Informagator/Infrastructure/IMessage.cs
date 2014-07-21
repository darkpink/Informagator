using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IMessage
    {
        byte[] BinaryData { get; set; }
        IDictionary<string, string> Attributes { get; }
        IList<string> ProcessingTrail { get; }
    }

    public interface IMessage<TBody> : IMessage
    {
        TBody Body { get; set; }
    }
}
