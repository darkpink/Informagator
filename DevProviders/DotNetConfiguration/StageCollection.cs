using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public  class StageCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Stage();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Stage)element).Name;
        }
    }
}
