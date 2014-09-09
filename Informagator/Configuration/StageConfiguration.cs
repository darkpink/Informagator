using Acadian.Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    [Serializable]
    [DataContract]
    public class StageConfiguration
    {
        [DataMember]
        public string StageAssemblyName { get; set; }

        [DataMember]
        public string StageType { get; set; }

        [DataMember]
        public string ErrorHandlerAssemblyName { get; set; }

        [DataMember]
        public string ErrorHandlerType { get; set; }
        
        [DataMember]
        public bool IsTrackingEnabled { get; set; }

        [DataMember]
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
