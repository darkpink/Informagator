using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DBEntities.Configuration
{
    public partial class WorkerErrorHandler : IErrorHandlerConfiguration
    {
        public string AssemblyName
        {
            get 
            {
                return ErrorHandler == null ? null :
                    ErrorHandler.Assembly == null ? null :   
                    ErrorHandler.Assembly.Name;
            }
        }

        public string AssemblyVersion
        {
            get 
            {
                return ErrorHandler == null ? null :
                    ErrorHandler.Assembly == null ? null : 
                    ErrorHandler.Assembly.Version;
            }
        }

        public string Type
        {
            get 
            {
                return ErrorHandler == null ? null : ErrorHandler.Type;
            }
        }

        public IList<IConfigurationParameter> Parameters
        {
            get 
            {
                return ErrorHandler == null ? null : 
                    ErrorHandler.Parameters == null ? null :
                    ErrorHandler.Parameters.Cast<IConfigurationParameter>().ToList();
            }
        }
    }
}
