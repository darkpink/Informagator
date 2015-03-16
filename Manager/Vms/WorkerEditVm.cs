using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Editor = Informagator.Manager.Controls.StageEditor;
using System.Collections.ObjectModel;

namespace Informagator.Manager.Vms
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
            WorkerAssemblyId = worker.Assembly.Id;
            LoadStages(worker);
            return worker;
        }

        private void LoadStages(Worker workerEntity)
        {
            Stages.Clear();

            foreach (Stage stg in workerEntity.Stages)
            {
                Editor.Stage editorStage = new Editor.Stage();
                editorStage.Name = stg.Name;
                editorStage.StageAssemblyId = stg.Assembly.Id;
                editorStage.StageType = stg.Type;
                foreach (StageParameter param in stg.StageParameters)
                {
                    Editor.StageParameter editorParam = new Editor.StageParameter();
                    editorStage.StageParameters.Add(editorParam);
                    editorParam.Name = param.Name;
                    editorParam.Value = param.Value;
                }
                Stages.Add(editorStage);
            }
        }

        private void SaveStages()
        {
            while(Entity.Stages.Count < Stages.Count)
            {
                Entity.Stages.Add(new Stage() { Worker = Entity });
            }

            foreach(Stage toDelete in Entity.Stages.Skip(Stages.Count).ToList())
            {
                toDelete.StageParameters.Clear();
                Entity.Stages.Remove(toDelete);
            }

            for(int index = 0; index < Stages.Count; index++)
            {
                Stage dbStage = Entity.Stages.ElementAt(index);
                Editor.Stage uiStage = Stages[index];
                dbStage.Sequence = index;
                dbStage.Name = uiStage.Name;
                Assembly stageAssemblyVersion = Entities.Assemblies.SingleOrDefault(av => av.Id == uiStage.StageAssemblyId);
                dbStage.Assembly = stageAssemblyVersion;
                dbStage.Type = uiStage.StageType;

                foreach (var uiParam in uiStage.StageParameters)
                {
                    var dbParam = dbStage.StageParameters.SingleOrDefault(p => p.Name == uiParam.Name);
                    if (dbParam == null)
                    {
                        dbParam = Entities.StageParameters.Create();
                        dbParam.Stage = dbStage;
                        Entities.StageParameters.Add(dbParam);
                        dbParam.Name = uiParam.Name;
                    }

                    dbParam.Value = uiParam.Value.ToString();
                }

                dbStage.StageParameters.Where(dbp => !uiStage.StageParameters.Any(uip => uip.Name == dbp.Name))
                       .ToList()
                       .ForEach(p => Entities.StageParameters.Remove(p));
            }
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

        private long? _workerAssemblyId;
        public long? WorkerAssemblyId
        {
            get
            {
                return _workerAssemblyId;
            }
            set
            {
                _workerAssemblyId = value;
                NotifyPropertyChanged("WorkerAssemblyId");
                AttemptToSetAssembly();
            }
        }

        public WorkerEditVm()
        {
            Stages = new ObservableCollection<Editor.Stage>();
        }
        public override void SaveEntity()
        {
            SaveStages();
            base.SaveEntity();
        }
        
        private void AttemptToSetAssembly()
        {
            if (Entity != null && WorkerAssemblyId != null)
            {
                Assembly workerAssembly = Entities.Assemblies.SingleOrDefault(av => av.Id == WorkerAssemblyId);
                Entity.Assembly = workerAssembly;
            }
        }
    }
}
