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
        byte[] Body { get; }
        IDictionary<string, string> Attributes { get; }
        IList<string> ProcessingTrail { get; }
    }
}
