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
                return ErrorHandlerParameters == null ? null : ErrorHandlerParameters.Cast<IConfigurationParameter>().ToList();
            }
        }
    }
}
