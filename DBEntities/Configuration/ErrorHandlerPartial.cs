using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DBEntities.Configuration
{
    public partial class ErrorHandler : IErrorHandlerConfiguration
    {
        //TODO protect against nullreferenceexceptions when not loaded
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
                return ErrorHandlerParameters.Cast<IConfigurationParameter>().ToList();
            }
        }
    }
}
