using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class WorkerParameter : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey=true, IsRequired=true)]
        public string Name
        {
            get
            {
                return base["name"] as string;
            }
        }

        [ConfigurationProperty("value", IsRequired=true)]
        public string Value
        {
            get
            {
                return base["value"] as string;
            }
        }
    }
}
