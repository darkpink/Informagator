using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Contracts
{
    public interface IConfigurationProvider
    {
        IInformagatorConfiguration Configuration { get; }
    }
}
