using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Manager.Vms
{
    public abstract class SelectedConfigurationVmBase : VmBase
    {
        public SelectedConfigurationVmBase()
            : base()
        {
            (App.Current as App).ActiveSystemConfigurationChanged += ActiveSystemConfigurationChanged;
        }

        private static string _selectedConfiguration; //static - shared across the panel vms
        public string SelectedConfiguration
        {
            get
            {
                return _selectedConfiguration;
            }
            set
            {
                if (value != _selectedConfiguration)
                {
                    _selectedConfiguration = value;
                    NotifyPropertyChanged("SelectedConfiguration");
                    (App.Current as App).NotifyActiveSystemConfigurationChanged();
                }
            }
        }

        protected virtual void ActiveSystemConfigurationChanged()
        {

        }
    
    }
}
