using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager.Vms
{
    public class MachineEditVm : EntityEditVmBase<Informagator.DBEntities.Configuration.Machine>
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

        protected override bool IsValid
        {
            get { return true; }
        }

        protected override Informagator.DBEntities.Configuration.Machine LoadEntity()
        {
            var entity = Entities.Machines.Single(m => m.Id == EntityId);

            LoadErrorHandlers(entity);

            return entity;
        }

        public override void SaveEntity()
        {
            SaveErrorHandlers();
            base.SaveEntity();
        }

        private void LoadErrorHandlers(Machine workerEntity)
        {
            ErrorHandlerIds.Clear();

            foreach (MachineErrorHandler handler in workerEntity.MachineErrorHandlers)
            {
                ErrorHandlerIds.Add(handler.ErrorHandler.Id);
            }
        }

        private void SaveErrorHandlers()
        {
            foreach (MachineErrorHandler existingErrorHandler in Entity.MachineErrorHandlers.ToList())
            {
                if (!ErrorHandlerIds.Contains((long)(existingErrorHandler.ErrorHandler.Id)))
                {
                    Entities.MachineErrorHandlers.Remove(existingErrorHandler);
                }
            }

            foreach (long id in ErrorHandlerIds.Where(id => id != null).Cast<long>().Distinct())
            {
                if (!Entity.MachineErrorHandlers.Any(weh => weh.ErrorHandler.Id == id))
                {
                    MachineErrorHandler newErrorHandler = new MachineErrorHandler();
                    newErrorHandler.ErrorHandler = Entities.ErrorHandlers.Single(eh => eh.Id == id);
                    newErrorHandler.Machine = Entity;
                    Entities.MachineErrorHandlers.Add(newErrorHandler);
                }
            }
        }


        protected override Informagator.DBEntities.Configuration.Machine CreateNewEntity()
        {
            var mach = Entities.Machines.Create();
            mach.SystemConfiguration = Entities.SystemConfigurations.Single(sc => sc.Name == ConfigurationSelection.SelectedConfiguration);
            Entities.Machines.Add(mach);
            return mach;
        }

        public MachineEditVm()
            : base()
        {
            ErrorHandlerIds = new ObservableCollection<long?>();
        }
    }
}
