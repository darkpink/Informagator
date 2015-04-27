using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class StageParameterCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new StageParameter();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StageParameter)element).Name;
        }
    }
}
