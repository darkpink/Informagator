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
        //TODO protect against nullreferenceexceptions when not fully loaded
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

        public IList<IConfigurationParameter> Parameters
        {
            get 
            {
                return this.StageParameters.Cast<IConfigurationParameter>().ToList();
            }
        }

        public IList<IErrorHandlerConfiguration> ErrorHandlers
        {
            get 
            {
                return StageErrorHandlers.Cast<IErrorHandlerConfiguration>().ToList();
            }
        }
    }
}
