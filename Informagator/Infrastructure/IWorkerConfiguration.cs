using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Infrastructure
{
    public interface IWorkerConfiguration
    {
        string Name { get; }

        IList<IStageConfiguration> StageConfigurations { get; }
    }
}
