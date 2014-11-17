using Acadian.Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Acadian.Informagator.Manager.Vms
{
    public class WorkerEditVm : EntityEditVmBase<Worker>
    {
        protected override bool IsValid
        {
            get { return true; }
        }
        protected override Worker LoadEntity()
        {
            var worker = Entities.Workers.Include(w => w.Machine).Single(w => w.Id == EntityId);
            MachineName = worker.Machine.Name;
            AssemblyName = worker.WorkerAssemblyName;
            AssemblyDotNetVersion = worker.WorkerAssemblyDotNetVersion;
            return worker;
        }

        protected override Worker CreateNewEntity()
        {
            var worker = Entities.Workers.Create();
            Entities.Workers.Add(worker);
            return worker;
        }

        private string _machineName;
        public string MachineName
        {
            get
            {
                return _machineName;
            }
            set
            {
                _machineName = value;
                NotifyPropertyChanged("MachineName");

                if (Entity != null)
                {
                    var mach = Entities.SystemConfigurations
                                       .Where(sc => sc.Description == SelectedConfiguration)
                                       .SelectMany(sc => sc.Machines)
                                       .SingleOrDefault(m => m.Name == value);
                    if (mach != null)
                    {
                        Entity.Machine = mach;
                    }
                }
            }
        }

        private string _assemblyName;
        public string AssemblyName
        {
            get
            {
                return _assemblyName;
            }
            set
            {
                _assemblyName = value;
                NotifyPropertyChanged("AssemblyName");
                AttemptToSetAssembly();
            }
        }

        private string _assemblyDotNetVersion;
        public string AssemblyDotNetVersion
        {
            get
            {
                return _assemblyDotNetVersion;
            }
            set
            {
                _assemblyDotNetVersion = value;
                NotifyPropertyChanged("AssemblyDotNetVersion");
                AttemptToSetAssembly();
            }
        }

        private void AttemptToSetAssembly()
        {
            if (Entity != null && AssemblyName != null && AssemblyDotNetVersion != null)
            {
                var asm = Entities.SystemConfigurations
                          .Include(sc => sc.AssemblySystemConfigurations
                                           .Select(asc => asc.Assembly))
                          .Single(c => c.Description == SelectedConfiguration)
                          .AssemblySystemConfigurations.Select(asc => asc.Assembly)
                          .SingleOrDefault(a => a.DotNetVersion == AssemblyDotNetVersion && a.Name == AssemblyName);
                if (asm != null)
                {
                    Entity.Assembly = asm;
                }
            }
        }
    }
}
