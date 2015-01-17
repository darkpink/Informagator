using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.Configuration
{
    public class HardCodedStageConfigurationParameter : IStageConfigurationParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsSameAs(IStageConfigurationParameter param)
        {
            return Name == param.Name && Value == param.Value;
        }
    }
}
