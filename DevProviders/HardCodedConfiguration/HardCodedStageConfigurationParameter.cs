using Informagator.Contracts;
using Informagator.Contracts.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.HardCodedConfiguration
{
    public class HardCodedStageConfigurationParameter : IConfigurationParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
