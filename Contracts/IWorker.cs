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
        
        IThreadStatus Status { get; }

        IWorkerConfiguration Configuration { set; }

        bool IsRestartRequiredForNewConfiguration(IWorkerConfiguration newConfiguration);

        void ValidateSettings();
    }
}
