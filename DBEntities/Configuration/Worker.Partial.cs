using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DBEntities.Configuration
{
    public partial class Worker : IWorkerConfiguration
    {
        //TODO: protect against nullreferenceexceptions when not loaded
        IList<IStageConfiguration> IWorkerConfiguration.Stages
        {
            get 
            {
                return Stages.Cast<IStageConfiguration>().ToList();
            }
        }

        public IList<IConfigurationParameter> Parameters
        {
            get 
            {
                return WorkerParameters.Cast<IConfigurationParameter>().ToList();
            }
        }

        public string AssemblyName
        {
            get 
            {
                return Assembly.Name;
            }
        }

        public string AssemblyVersion
        {
            get 
            {
                return Assembly.Version;
            }
        }
    }
}
