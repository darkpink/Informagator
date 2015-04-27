using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class ErrorHandler : ConfigurationElement, IErrorHandlerConfiguration
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }

        [ConfigurationProperty("assemblyName", IsKey=true, IsRequired=true)]
        public string AssemblyName
        {
            get
            {
                return (string)base["assemblyName"];
            }
        }

        [ConfigurationProperty("assemblyName", IsRequired=true)]
        public string Assembly
        {
            get
            {
                return (string)base["assembly"];
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
        [ConfigurationCollection(typeof(ErrorHandlerParameterCollection), AddItemName="parameter")]
        public ErrorHandlerParameterCollection Parameters
        {
            get
            {
                return (ErrorHandlerParameterCollection)base["parameters"];
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
