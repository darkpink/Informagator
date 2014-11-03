using Acadian.Informagator.Manager.Vms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Acadian.Informagator.Manager.Commands
{
    public class CancelEditCommand<T> : ICommand
    {
        protected EntityEditVmBase<T> ViewModel { get; set; }

        private bool _allowExecute = true;

        public bool AllowExecute
        {
            private get
            {
                return _allowExecute;
            }
            set
            {
                _allowExecute = value;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            return AllowExecute;
        }

        public event EventHandler CanExecuteChanged;

        public CancelEditCommand(EntityEditVmBase<T> viewModel)
        {
            ViewModel = viewModel;
        }

        public void Execute(object parameter)
        {
            ViewModel.CancelEditEntity();
        }
    }
}
