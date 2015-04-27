using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class ErrorHandlerParameterCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ErrorHandlerParameter();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ErrorHandlerParameter)element).Name;
        }
    }
}
