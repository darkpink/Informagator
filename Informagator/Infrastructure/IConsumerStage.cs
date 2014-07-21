using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IConsumerStage
    {
        void ConsumeMessage(IMessage message);
    }
}
