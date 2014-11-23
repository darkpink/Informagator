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
            WorkerAssemblyName = worker.WorkerAssemblyVersion.AssemblyName;
            WorkerAssemblyDotNetVersion = worker.WorkerAssemblyVersion.AssemblyDotNetVersion;
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
                editorStage.StageAssemblyName = stg.StageAssemblyVersion.AssemblyName;
                editorStage.StageAssemblyDotNetVersion = stg.StageAssemblyVersion.AssemblyDotNetVersion;
                editorStage.StageType = stg.StageType;
                editorStage.ErrorHandlerAssemblyName = stg.ErrorHandlerAssemblyVersion.AssemblyName;
                editorStage.ErrorHandlerAssemblyDotNetVersion = stg.ErrorHandlerAssemblyVersion.AssemblyDotNetVersion;
                editorStage.ErrorHandlerType = stg.ErrorHandlerType;
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
                Entity.Stages.Add(new Stage());
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
                AssemblyVersion stageAssemblyVersion = Entities.AssemblyVersions.SingleOrDefault(av => av.AssemblyName == uiStage.StageAssemblyName &&
                                                                                      av.AssemblyDotNetVersion == uiStage.StageAssemblyDotNetVersion &&
                                                                                      av.AssemblySystemConfigurations.Any(asc => asc.SystemConfiguration.Description == SelectedConfiguration)
                                                                                );
                dbStage.StageAssemblyVersion = stageAssemblyVersion;
                dbStage.StageType = uiStage.StageType;

                AssemblyVersion errorHandlerAssemblyVersion = Entities.AssemblyVersions.SingleOrDefault(av => av.AssemblyName == uiStage.ErrorHandlerAssemblyName &&
                                                                      av.AssemblyDotNetVersion == uiStage.ErrorHandlerAssemblyDotNetVersion &&
                                                                      av.AssemblySystemConfigurations.Any(asc => asc.SystemConfiguration.Description == SelectedConfiguration)
                                                                );

                dbStage.ErrorHandlerAssemblyVersion = errorHandlerAssemblyVersion;
                dbStage.ErrorHandlerType = uiStage.ErrorHandlerType;

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
                       .ForEach(p => dbStage.StageParameters.Remove(p));

                foreach (var uiParam in uiStage.ErrorHandlerParameters)
                {
                    var dbParam = dbStage.ErrorHandlerParameters.SingleOrDefault(p => p.Name == uiParam.Name);
                    if (dbParam == null)
                    {
                        dbParam = Entities.ErrorHandlerParameters.Create();
                        dbParam.Stage = dbStage;
                        Entities.ErrorHandlerParameters.Add(dbParam);
                        dbParam.Name = uiParam.Name;
                    }

                    dbParam.Value = uiParam.Value.ToString();
                }

                dbStage.ErrorHandlerParameters.Where(dbp => !uiStage.ErrorHandlerParameters.Any(uip => uip.Name == dbp.Name))
                       .ToList()
                       .ForEach(p => dbStage.ErrorHandlerParameters.Remove(p));

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

        private string _workerAssemblyName;
        public string WorkerAssemblyName
        {
            get
            {
                return _workerAssemblyName;
            }
            set
            {
                _workerAssemblyName = value;
                NotifyPropertyChanged("AssemblyName");
                AttemptToSetAssembly();
            }
        }

        private string _workerAssemblyDotNetVersion;
        public string WorkerAssemblyDotNetVersion
        {
            get
            {
                return _workerAssemblyDotNetVersion;
            }
            set
            {
                _workerAssemblyDotNetVersion = value;
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
            SaveStages();
            base.SaveEntity();
        }
        private void AttemptToSetAssembly()
        {
            if (Entity != null && WorkerAssemblyName != null && WorkerAssemblyDotNetVersion != null)
            {
                AssemblyVersion workerAssemblyVersion = Entities.AssemblyVersions.SingleOrDefault(av => av.AssemblyName == WorkerAssemblyName &&
                                                                        av.AssemblyDotNetVersion == WorkerAssemblyDotNetVersion &&
                                                                        av.AssemblySystemConfigurations.Any(asc => asc.SystemConfiguration.Description == SelectedConfiguration)
                                                                );
                Entity.WorkerAssemblyVersion = workerAssemblyVersion;
            }
        }
    }
}
