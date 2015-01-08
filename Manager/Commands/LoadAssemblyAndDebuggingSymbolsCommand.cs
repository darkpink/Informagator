using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Commands
{
    public class LoadAssemblyAndDebuggingSymbolsCommand : ICommand
    {
        Action<string> FileSelected { get; set; }
        public LoadAssemblyAndDebuggingSymbolsCommand(Action<string> fileSelected)
        {
            FileSelected = fileSelected;
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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "Assembly Files (*.dll) | *.dll";
            if (dialog.ShowDialog() == true)
            {
                FileSelected(dialog.FileName);
            }
        }
    }
}
