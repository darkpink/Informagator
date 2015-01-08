using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager.Vms
{
    public abstract class ListPanelVmBase<T> : SelectedConfigurationVmBase
    {
        public ListPanelVmBase()
            : base()
        {
        }

        public override void Refresh()
        {
            base.Refresh();

            Entities = GetEntities();
        }

        private T[] _entities;
        public T[] Entities
        { 
            get
            {
                if (_entities == null)
                {
                    _entities = GetEntities();
                }

                return _entities;
            }
            
            protected set
            {
                _entities = value;
                NotifyPropertyChanged("Entities");
            }

        }
        protected abstract T[] GetEntities();

        protected override void ActiveSystemConfigurationChanged()
        {
            base.ActiveSystemConfigurationChanged();
            Entities = GetEntities();
        }
    }
}
