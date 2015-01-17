using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Stages
{
    public interface ISupplierStage : IProcessingStage
    {
        IMessage Supply();
        string ReceviedFrom { get; }
    }
}
