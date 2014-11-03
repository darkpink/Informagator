using Acadian.Informagator.Manager.Commands;
using Acadian.Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Acadian.Informagator.Manager.Vms
{
    public abstract class EntityEditVmBase<T> : VmBase
    {
        protected ConfigurationEntities Entities { get; set; }
        protected long? EntityId { get; set; }
        public ObservableCollection<string> ValidationErrors { get; protected set; }
        protected abstract bool IsValid { get; }

        public T Entity { get; protected set; }
        public override object Parameter
        {
            get
            {
                return base.Parameter;
            }
            set
            {
                base.Parameter = value;
                Entities = new ConfigurationEntities();
                ValidationErrors = new ObservableCollection<string>();
                EntityId = value as long?;
                Entity = EntityId == null ? CreateNewEntity() : LoadEntity();
            }
        }

        public ICommand Save { get { return new SaveEntityCommand<T>(this); } }
        public ICommand Cancel { get { return new CancelEditCommand<T>(this); } }
        protected abstract T LoadEntity();

        protected abstract T CreateNewEntity();
        public virtual void SaveEntity()
        {
            if (IsValid)
            {
                Entities.SaveChanges();
                Entities.Dispose();
                PanelChangeCommandManager.GoToPreviousView.Execute(null);
            }
        }
        
        public virtual void CancelEditEntity()
        {
            Entities.Dispose();
            PanelChangeCommandManager.GoToPreviousView.Execute(null);
        }
    }
}
