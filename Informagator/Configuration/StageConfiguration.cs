using Acadian.Informagator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    [Serializable]
    public class StageConfiguration : IStageConfiguration
    {
        public string StageAssemblyName { get; set; }
        public string StageType { get; set; }
        public string ErrorHandlerAssemblyName { get; set; }
        public string ErrorHandlerType { get; set; }
        public bool IsTrackingEnabled { get; set; }
        public IList<IStageConfigurationParameter> Parameters { get; set; }

        public StageConfiguration()
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

            foreach (StageConfigurationParameter param in Parameters)
            {
                StageConfigurationParameter matchingParam = config.Parameters.SingleOrDefault(p => p.Name == param.Name) as StageConfigurationParameter;
                result &= matchingParam != null && matchingParam.IsSameAs(param);
            }

            foreach (StageConfigurationParameter param in config.Parameters)
            {
                StageConfigurationParameter matchingParam = Parameters.SingleOrDefault(p => p.Name == param.Name) as StageConfigurationParameter;
                result &= matchingParam != null && matchingParam.IsSameAs(param);
            }

            return result;
        }
    }
}
