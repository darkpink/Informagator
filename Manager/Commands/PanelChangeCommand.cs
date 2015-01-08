using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Commands
{
    public class PanelChangeCommand : ICommand
    {
        protected Action<PanelChangeCommandManager.PanelView, object> ToInvoke { get; set; }
        protected PanelChangeCommandManager.PanelView SwitchTo { get; set; }

        public PanelChangeCommand(PanelChangeCommandManager.PanelView switchTo, Action<PanelChangeCommandManager.PanelView, object> toInvoke)
        {
            ToInvoke = toInvoke;
            SwitchTo = switchTo;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        #pragma warning disable 67
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 67

        public void Execute(object parameter)
        {
            ToInvoke(SwitchTo, parameter);
        }
    }
}
