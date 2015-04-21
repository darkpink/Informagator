using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DBEntities.Configuration
{
    public partial class Stage : IStageConfiguration
    {
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

        public IList<IConfigurationParameter> Parameters
        {
            get 
            {
                return StageParameters == null ? null : StageParameters.Cast<IConfigurationParameter>().ToList();
            }
        }

        public IList<IErrorHandlerConfiguration> ErrorHandlers
        {
            get 
            {
                return StageErrorHandlers == null ? null : StageErrorHandlers.Cast<IErrorHandlerConfiguration>().ToList();
            }
        }
    }
}
