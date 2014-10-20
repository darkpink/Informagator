using Acadian.Informagator.Manager.ManagementItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Manager
{
    public class ManagementItemCache
    {
        public List<MachineDefinition> Machines { get; set; }
        
        public List<ProcessDefinition> Processes  { get; set; }
        
        public List<AssemblyDefinition> Assemblies { get; set; }
    }
}
