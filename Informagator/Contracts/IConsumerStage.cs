using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Contracts
{
    public interface IConsumerStage : IProcessingStage
    {
        string Consume(IMessage message); //returns where it was sent
    }
}
