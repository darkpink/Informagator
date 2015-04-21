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
        IList<IStageConfiguration> IWorkerConfiguration.Stages
        {
            get 
            {
                return Stages == null ? null : Stages.Cast<IStageConfiguration>().ToList();
            }
        }

        public IList<IConfigurationParameter> Parameters
        {
            get 
            {
                return WorkerParameters == null ? null : WorkerParameters.Cast<IConfigurationParameter>().ToList();
            }
        }

        public string AssemblyName
        {
            get 
            {
                return Assembly == null ? null : Assembly.Name;
            }
        }

        public string AssemblyVersion
        {
            get 
            {
                return Assembly == null ? null : Assembly.Version;
            }
        }

        public IList<IErrorHandlerConfiguration> ErrorHandlers
        {
            get { return this.WorkerErrorHandlers == null ? null : WorkerErrorHandlers.Cast<IErrorHandlerConfiguration>().ToList(); }
        }
    }
}
