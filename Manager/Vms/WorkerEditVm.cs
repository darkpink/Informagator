using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Editor = Informagator.Manager.Controls;
using System.Collections.ObjectModel;

namespace Informagator.Manager.Vms
{
    public class WorkerEditVm : EntityEditVmBase<Worker>
    {
        private ObservableCollection<long?> _errorHandlerIds;
        public ObservableCollection<long?> ErrorHandlerIds
        {
            get
            {
                return _errorHandlerIds;
            }
            set
            {
                _errorHandlerIds = value;
                NotifyPropertyChanged("ErrorHandlerIds");
            }
        }

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
                                         .Include(w => w.WorkerErrorHandlers.Select(eh => eh.ErrorHandler))
                                         .Single(w => w.Id == EntityId);
            MachineId = worker.Machine.Id;
            WorkerAssemblyId = worker.Assembly.Id;
            LoadStages(worker);
            LoadErrorHandlers(worker);
            return worker;
        }

        private void LoadStages(Worker workerEntity)
        {
            Stages.Clear();

            foreach (Stage stg in workerEntity.Stages)
            {
                Editor.Stage editorStage = new Editor.Stage();
                editorStage.EntityName = stg.Name;
                editorStage.AssemblyId = stg.Assembly.Id;
                editorStage.EntityType = stg.Type;
                foreach (StageParameter param in stg.StageParameters)
                {
                    Editor.Parameter editorParam = new Editor.Parameter();
                    editorStage.Parameters.Add(editorParam);
                    editorParam.Name = param.Name;
                    editorParam.Value = param.Value;
                }
                Stages.Add(editorStage);
            }
        }

        private void LoadErrorHandlers(Worker workerEntity)
        {
            ErrorHandlerIds.Clear();

            foreach (WorkerErrorHandler handler in workerEntity.WorkerErrorHandlers)
            {
                ErrorHandlerIds.Add(handler.ErrorHandler.Id);
            }
        }

        private void SaveStages()
        {
            //possible violations of unique constraints on sequence and name
            foreach(Stage toDelete in Entity.Stages.Where(s => !Enumerable.Range(0, Stages.Count).Any(index => Stages[index].EntityName == s.Name && index == s.Sequence)).ToList())
            {
                Entities.StageParameters.RemoveRange(toDelete.StageParameters);
                Entities.Stages.Remove(toDelete);
            }

            Entities.SaveChanges();

            while (Entity.Stages.Count < Stages.Count)
            {
                Entity.Stages.Add(new Stage() { Worker = Entity });
            }

            for (int index = 0; index < Stages.Count; index++)
            {
                Editor.Stage uiStage = Stages[index];
                Stage dbStage = Entity.Stages.FirstOrDefault(s => s.Name == uiStage.EntityName);
                if (dbStage == null)
                {
                    dbStage = Entity.Stages.First(s => !Stages.Any(uis => s.Name == uis.EntityName));
                }

                dbStage.Sequence = index;
                dbStage.Name = uiStage.EntityName;
                Assembly stageAssemblyVersion = Entities.Assemblies.SingleOrDefault(av => av.Id == uiStage.AssemblyId);
                dbStage.Assembly = stageAssemblyVersion;
                dbStage.Type = uiStage.EntityType;

                foreach (var uiParam in uiStage.Parameters)
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

                dbStage.StageParameters.Where(dbp => !uiStage.Parameters.Any(uip => uip.Name == dbp.Name))
                       .ToList()
                       .ForEach(p => Entities.StageParameters.Remove(p));
            }
        }

        private void SaveErrorHandlers()
        {
            foreach(WorkerErrorHandler existingErrorHandler in Entity.WorkerErrorHandlers.ToList())
            {
                if (!ErrorHandlerIds.Contains((long)(existingErrorHandler.ErrorHandler.Id)))
                {
                    Entities.WorkerErrorHandlers.Remove(existingErrorHandler);
                }
            }

            foreach(long id in ErrorHandlerIds.Where(id => id != null).Cast<long>().Distinct())
            {
                if (!Entity.WorkerErrorHandlers.Any(weh => weh.ErrorHandler.Id == id ))
                {
                    WorkerErrorHandler newErrorHandler = new WorkerErrorHandler();
                    newErrorHandler.ErrorHandler = Entities.ErrorHandlers.Single(eh => eh.Id == id);
                    newErrorHandler.Worker = Entity;
                    Entities.WorkerErrorHandlers.Add(newErrorHandler);
                }
            }
        }

        protected override Worker CreateNewEntity()
        {
            var worker = Entities.Workers.Create();
            Entities.Workers.Add(worker);
            return worker;
        }

        private long? _machineId;
        public long? MachineId
        {
            get
            {
                return _machineId;
            }
            set
            {
                _machineId = value;

                if (Entity != null)
                {
                    var mach = Entities.Machines.SingleOrDefault(m => m.Id == _machineId);
                    Entity.Machine = mach;
                }

                NotifyPropertyChanged("MachineId");
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
            ErrorHandlerIds = new ObservableCollection<long?>();
        }
        public override void SaveEntity()
        {
            SaveStages();
            SaveErrorHandlers();
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
