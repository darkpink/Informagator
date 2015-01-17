using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IWorker
    {
        void Start();
        void Stop();
        string Name { set; }
        IThreadStatus Status { get; }

        IThreadConfiguration Configuration { set; }

        IList<string> RequiredAssemblies { get; }

        void ValidateSettings();
    }
}
