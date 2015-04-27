using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class MachineCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Machine();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Machine)element).HostName;
        }
    }
}
