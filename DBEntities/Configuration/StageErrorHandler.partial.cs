using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DBEntities.Configuration
{
    public partial class StageErrorHandler : IErrorHandlerConfiguration
    {
        //TODO: protect against nullreferenceexceptions when not loaded
        public string AssemblyName
        {
            get 
            {
                return ErrorHandler.Assembly.Name;
            }
        }

        public string AssemblyVersion
        {
            get 
            {
                return ErrorHandler.Assembly.Version;
            }
        }

        public string Type
        {
            get 
            {
                return ErrorHandler.Type;
            }
        }

        public IList<IConfigurationParameter> Parameters
        {
            get 
            {
                return ErrorHandler.Parameters.OfType<IConfigurationParameter>().ToList();
            }
        }
    }
}
