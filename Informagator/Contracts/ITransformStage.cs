using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    public interface ITransformStage : IProcessingStage
    {
        IEnumerable<IMessage> TransformMessage(IMessage message);
    }
}
