using Informagator.DBEntities.Configuration;
using Informagator.Manager.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Vms
{
    public class AssemblyListVm : ListPanelVmBase<Assembly>
    {
        public ICommand DeleteAssembly
        {
            get
            {
                return new DeleteEntityCommand<Assembly>(entities => entities.Assemblies, (Assemblies, id) => Assemblies.Single(w => w.Id == (long)id), null, Refresh);
            }
        }

        protected override Assembly[] GetEntities()
        {
            Assembly[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.Assemblies
                                 .Where(av => av.SystemConfiguration.Name == ConfigurationSelection.SelectedConfiguration)
                                 .ToArray();
            }

            return result;
        }

    }
}
