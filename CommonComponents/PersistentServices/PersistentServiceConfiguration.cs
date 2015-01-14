using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.PersistentServices
{
    public class PersistentServiceConfiguration : IPersistentServiceConfiguration
    {
        protected string Signature { get; set; }
        public PersistentServiceConfiguration(string signature)
        {

        }
        public string ConfigurationSignature
        {
            get 
            {
                return Signature;
            }
        }
    }
}
