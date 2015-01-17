using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.Configuration
{
    public class HardCodedStageConfiguration : IStageConfiguration
    {
        public string StageAssemblyName { get; set; }

        public string StageType { get; set; }

        public string ErrorHandlerAssemblyName { get; set; }

        public string ErrorHandlerType { get; set; }
        
        public bool IsTrackingEnabled { get; set; }

        public IList<IStageConfigurationParameter> Parameters { get; set; }

        public HardCodedStageConfiguration()
        {
            Parameters = new List<IStageConfigurationParameter>();
        }

        public bool IsSameAs(IStageConfiguration config)
        {
            bool result = true;

            result &= StageAssemblyName == config.StageAssemblyName;
            result &= StageType == config.StageType;
            result &= ErrorHandlerAssemblyName == config.ErrorHandlerAssemblyName;
            result &= ErrorHandlerType == config.ErrorHandlerType;
            result &= IsTrackingEnabled == config.IsTrackingEnabled;

            foreach (HardCodedStageConfigurationParameter param in Parameters)
            {
                HardCodedStageConfigurationParameter matchingParam = config.Parameters.SingleOrDefault(p => p.Name == param.Name) as HardCodedStageConfigurationParameter;
                result &= matchingParam != null && matchingParam.IsSameAs(param);
            }

            foreach (HardCodedStageConfigurationParameter param in config.Parameters)
            {
                HardCodedStageConfigurationParameter matchingParam = Parameters.SingleOrDefault(p => p.Name == param.Name) as HardCodedStageConfigurationParameter;
                result &= matchingParam != null && matchingParam.IsSameAs(param);
            }

            return result;
        }
    }
}
