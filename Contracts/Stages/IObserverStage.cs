using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Stages
{
    public interface IObserverStage : IProcessingStage
    {
        void Observe(IMessage message);
    }
}
