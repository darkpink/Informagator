using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Configuration
{
    [Serializable]
    [DataContract]
    public class StageConfigurationParameter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        public bool IsSameAs(StageConfigurationParameter param)
        {
            return Name == param.Name && Value == param.Value;
        }
    }
}
