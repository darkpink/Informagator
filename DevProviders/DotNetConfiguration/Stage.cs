using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class Stage : ConfigurationElement, IStageConfiguration
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }

        [ConfigurationProperty("assemblyName", IsRequired = true)]
        public string AssemblyName
        {
            get
            {
                return (string)base["assemblyName"];
            }
        }

        [ConfigurationProperty("assemblyVersion")]
        public string AssemblyVersion
        {
            get
            {
                return (string)base["assemblyVersion"];
            }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)base["type"];
            }
        }

        [ConfigurationProperty("parameters")]
        [ConfigurationCollection(typeof(StageParameterCollection), AddItemName = "parameter")]
        public StageParameterCollection Parameters
        {
            get
            {
                return (StageParameterCollection)base["parameters"];
            }
        }

        [ConfigurationProperty("errorHandlers")]
        [ConfigurationCollection(typeof(ErrorHandlerCollection), AddItemName = "errorHandler")]
        public ErrorHandlerCollection ErrorHandlers
        {
            get
            {
                return (ErrorHandlerCollection)base["errorHandlers"];
            }
        }

        [ConfigurationProperty("suppressParentErrorHandlers")]
        public bool SuppressParentErrorHandlers
        {
            get 
            { 
                return (bool)base["suppressParentErrorHandlers"]; 
            }
        }

        IList<IErrorHandlerConfiguration> IStageConfiguration.ErrorHandlers
        {
            get 
            { 
                return ErrorHandlers.OfType<IErrorHandlerConfiguration>().ToList(); 
            }
        }

        IList<IConfigurationParameter> IConfigurableTypeConfiguration.Parameters
        {
            get 
            { 
                return Parameters.OfType<IConfigurationParameter>().ToList(); 
            }
        }
    }
}
