using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class ErrorHandlerCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ErrorHandler();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ErrorHandler)element).Name;
        }
    }
}
