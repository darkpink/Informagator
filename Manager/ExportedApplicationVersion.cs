using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Manager
{
    [Serializable]
    public class ExportedApplicationVersion
    {
        public string Description { get; set; }

        public ExportedHostConfiguration[] Hosts { get; set; }
    }
}
