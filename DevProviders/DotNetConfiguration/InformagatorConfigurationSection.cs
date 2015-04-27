using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DevProviders.DotNetConfiguration
{
    public class InformagatorConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("machines", IsRequired=true)]
        [ConfigurationCollection(typeof(MachineCollection), AddItemName="machine")]
        public MachineCollection Machines
        {
            get
            {
                return (MachineCollection)base["machines"];
            }
        }


        [ConfigurationProperty("errorHandlers")]
        [ConfigurationCollection(typeof(ErrorHandlerCollection), AddItemName="errorHandler")]
        public ErrorHandlerCollection ErrorHandlers
        {
            get
            {
                return (ErrorHandlerCollection)base["errorHandlers"];
            }
        }
    }
}
