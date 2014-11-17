using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    public class ConfigurationParameterAttribute : Attribute
    {
        public string DisplayName { get; set; }
    }
}
