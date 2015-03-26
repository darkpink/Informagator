using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager.Vms
{
    public class AssemblyListVm : ListPanelVmBase<Assembly>
    {
        protected override Assembly[] GetEntities()
        {
            Assembly[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.Assemblies
                                 .Where(av => av.SystemConfiguration.Description == ConfigurationSelection.SelectedConfiguration)
                                 .ToArray();
            }

            return result;
        }

    }
}
