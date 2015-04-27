using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class WorkerParameterCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WorkerParameter();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WorkerParameter)element).Name;
        }
    }
}
