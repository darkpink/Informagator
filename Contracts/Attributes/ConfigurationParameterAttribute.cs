﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Attributes
{
    public class ConfigurationParameterAttribute : Attribute
    {
        public string DisplayName { get; set; }
    }
}
