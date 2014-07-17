using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Configuration
{
    public interface IConfigurationProvider
    {
        IInformagatorConfiguration Configuration { get; }
    }
}
