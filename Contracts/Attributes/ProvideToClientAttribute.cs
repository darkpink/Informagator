using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
    public class ProvideToClientAttribute : Attribute
    {
        public Type InterfaceType { get; private set; }
        
        public ProvideToClientAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
    }
}
