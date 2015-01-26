using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager.Vms
{
    public class MachineEditVm : EntityEditVmBase<Informagator.DBEntities.Configuration.Machine>
    {
        protected override bool IsValid
        {
            get { return true; }
        }

        protected override Informagator.DBEntities.Configuration.Machine LoadEntity()
        {
            return Entities.Machines.Single(m => m.Id == EntityId);
        }

        protected override Informagator.DBEntities.Configuration.Machine CreateNewEntity()
        {
            var mach = Entities.Machines.Create();
            mach.SystemConfiguration = Entities.SystemConfigurations.Single(sc => sc.Description == SelectedConfiguration);
            Entities.Machines.Add(mach);
            return mach;
        }
    }
}
