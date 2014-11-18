using Acadian.Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Editor = Acadian.Informagator.Manager.Controls.StageEditor;
using System.Collections.ObjectModel;

namespace Acadian.Informagator.Manager.Vms
{
    public class WorkerEditVm : EntityEditVmBase<Worker>
    {
        private ObservableCollection<Editor.Stage> _stages;
        public ObservableCollection<Editor.Stage> Stages
        {
            get
            {
                return _stages;
            }
            set
            {
                _stages = value;
                NotifyPropertyChanged("Stages");
            }
        }
        protected override bool IsValid
        {
            get { return true; }
        }
        protected override Worker LoadEntity()
        {
            var worker = Entities.Workers.Include(w => w.Machine)
                                         .Include(w => w.Stages.Select(s => s.StageParameters))
                                         .Single(w => w.Id == EntityId);
            MachineName = worker.Machine.Name;
            AssemblyName = worker.WorkerAssemblyName;
            AssemblyDotNetVersion = worker.WorkerAssemblyDotNetVersion;
            LoadStages(worker);
            return worker;
        }

        private void LoadStages(Worker workerEntity)
        {
            foreach(Stage stg in workerEntity.Stages)
            {
                Editor.Stage editorStage = new Editor.Stage();
                editorStage.Name = stg.Name;
                editorStage.StageAssemblyName = stg.StageAssemblyName;
                editorStage.StageAssemblyDotNetVersion = stg.StageAssemblyDotNetVersion;
                editorStage.StageType = stg.StageType;
                editorStage.ErrorHandlerAssemblyName = stg.ErrorHandlerAssemblyName;
                editorStage.ErrorHandlerAssemblyDotNetVersion = stg.ErrorHandlerAssemblyDotNetVersion;
                editorStage.ErrorHandlerType = stg.ErrorHandlerType;
                foreach(StageParameter param in stg.StageParameters)
                {
                    Editor.StageParameter editorParam = new Editor.StageParameter();
                    editorStage.StageParameters.Add(editorParam);
                    editorParam.Name = param.Name;
                    editorParam.Value = param.Value;
                }
                Stages.Add(editorStage);
            }
            Stages.Add(new Editor.Stage() { StageAssemblyName = "Acadian.Informagator.dll " });
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

        public WorkerEditVm()
        {
            Stages = new ObservableCollection<Editor.Stage>();
        }
        public override void SaveEntity()
        {
            base.SaveEntity();
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
