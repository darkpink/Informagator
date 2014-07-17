using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    [Serializable]
    public class StageConfiguration
    {
        public string StageAssemblyName { get; set; }
        public string StageType { get; set; }
        public string ErrorHandlerAssemblyName { get; set; }
        public string ErrorHandlerType { get; set; }

        public IList<StageConfigurationParameter> Parameters { get; set; }

        public StageConfiguration()
        {
            Parameters = new List<StageConfigurationParameter>();
        }
        public bool IsSameAs(StageConfiguration config)
        {
            bool result = true;

            result &= StageAssemblyName == config.StageAssemblyName;
            result &= StageType == config.StageType;
            result &= ErrorHandlerAssemblyName == config.ErrorHandlerAssemblyName;
            result &= ErrorHandlerType == config.ErrorHandlerType;

            foreach (StageConfigurationParameter param in Parameters)
            {
                StageConfigurationParameter matchingParam = config.Parameters.SingleOrDefault(p => p.Name == param.Name);
                result &= matchingParam != null && matchingParam.IsSameAs(param);
            }

            foreach (StageConfigurationParameter param in config.Parameters)
            {
                StageConfigurationParameter matchingParam = Parameters.SingleOrDefault(p => p.Name == param.Name);
                result &= matchingParam != null && matchingParam.IsSameAs(param);
            }

            return result;
        }
    }
}
