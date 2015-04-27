using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class Worker : ConfigurationElement, IWorkerConfiguration
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
        [ConfigurationCollection(typeof(WorkerParameterCollection), AddItemName = "parameter")]
        public WorkerParameterCollection Parameters
        {
            get
            {
                return (WorkerParameterCollection)base["parameters"];
            }
        }

        [ConfigurationProperty("stages")]
        [ConfigurationCollection(typeof(StageCollection), AddItemName = "stage")]
        public StageCollection Stages
        {
            get
            {
                return (StageCollection)base["stages"];
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

        [ConfigurationProperty("autoStart")]
        public bool AutoStart
        {
            get 
            {
                return (bool)base["autoStart"];
            }
        }

        IList<IStageConfiguration> IWorkerConfiguration.Stages
        {
            get
            {
                return Stages.OfType<IStageConfiguration>().ToList();
            }
        }

        IList<IErrorHandlerConfiguration> IWorkerConfiguration.ErrorHandlers
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
