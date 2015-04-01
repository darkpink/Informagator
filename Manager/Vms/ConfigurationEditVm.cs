using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager.Vms
{
    public class ConfigurationEditVm : EntityEditVmBase<SystemConfiguration>
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

        protected override Informagator.DBEntities.Configuration.SystemConfiguration LoadEntity()
        {
            var entity = Entities.SystemConfigurations.Single(m => m.Id == EntityId);

            LoadErrorHandlers(entity);

            return entity;
        }

        public override void SaveEntity()
        {
            Entity.CreateDttm = DateTime.Now;
            SaveErrorHandlers();
            base.SaveEntity();
        }

        private void LoadErrorHandlers(SystemConfiguration workerEntity)
        {
            ErrorHandlerIds.Clear();

            foreach (SystemConfigurationErrorHandler handler in workerEntity.SystemConfigurationErrorHandlers)
            {
                ErrorHandlerIds.Add(handler.ErrorHandler.Id);
            }
        }

        private void SaveErrorHandlers()
        {
            foreach (SystemConfigurationErrorHandler existingErrorHandler in Entity.SystemConfigurationErrorHandlers.ToList())
            {
                if (!ErrorHandlerIds.Contains((long)(existingErrorHandler.ErrorHandler.Id)))
                {
                    Entities.SystemConfigurationErrorHandlers.Remove(existingErrorHandler);
                }
            }

            foreach (long id in ErrorHandlerIds.Where(id => id != null).Cast<long>().Distinct())
            {
                if (!Entity.SystemConfigurationErrorHandlers.Any(weh => weh.ErrorHandler.Id == id))
                {
                    SystemConfigurationErrorHandler newErrorHandler = new SystemConfigurationErrorHandler();
                    newErrorHandler.ErrorHandler = Entities.ErrorHandlers.Single(eh => eh.Id == id);
                    newErrorHandler.SystemConfiguration = Entity;
                    Entities.SystemConfigurationErrorHandlers.Add(newErrorHandler);
                }
            }
        }


        protected override Informagator.DBEntities.Configuration.SystemConfiguration CreateNewEntity()
        {
            var mach = Entities.SystemConfigurations.Create();
            Entities.SystemConfigurations.Add(mach);
            return mach;
        }

        public ConfigurationEditVm()
            : base()
        {
            ErrorHandlerIds = new ObservableCollection<long?>();
        }
    }
}
