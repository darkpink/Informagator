﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    [Serializable]
    public class StageConfigurationParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public bool IsSameAs(StageConfigurationParameter param)
        {
            return Name == param.Name && Value == param.Value;
        }
    }
}