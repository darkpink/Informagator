using Acadian.Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Manager.Vms
{
    public class AssemblyListVm : ListPanelVmBase<AssemblyVersion>
    {
        protected override AssemblyVersion[] GetEntities()
        {
            AssemblyVersion[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.AssemblyVersions
                                 .Where(av => av.AssemblySystemConfigurations
                                                .Where(asc => asc.SystemConfiguration.Description == SelectedConfiguration)
                                                .Any()
                                       )
                                 .ToArray();
            }

            return result;
        }

    }
}
