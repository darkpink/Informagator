using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Acadian.Informagator.Manager.Commands
{
    public class ParameterKeyedActionCommand : ICommand
    {
        protected IDictionary Actions { get; set; }
        public ParameterKeyedActionCommand(IDictionary actions)
        {
            Actions = actions;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        #pragma warning disable 67
        public event EventHandler CanExecuteChanged = null;
        #pragma warning restore 67

        public void Execute(object parameter)
        {
            if (Actions.Contains(parameter))
            {
                Action toDo = (Action)Actions[parameter];
                toDo();
            }
        }
    }
}
