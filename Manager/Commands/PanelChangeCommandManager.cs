using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Commands
{
    public static class PanelChangeCommandManager
    {
        public enum PanelView
        {
            ConfigurationList, MachineList, AssemblyList, WorkerList, StartStop,
            ConfigurationEdit, MachineEdit, AssemblyEdit, WorkerEdit,
            ErrorHandlerList, ErrorHandlerEdit
        };

        private static PanelView PreviousView { get; set; }
        private static PanelView CurrentView { get; set; }
        public static ICommand GoToConfigurationList { get { return new PanelChangeCommand(PanelView.ConfigurationList, RequestPanelChange); } }
        public static ICommand GoToMachineList { get { return new PanelChangeCommand(PanelView.MachineList, RequestPanelChange); } }
        public static ICommand GoToWorkerList { get { return new PanelChangeCommand(PanelView.WorkerList, RequestPanelChange); } }
        public static ICommand GoToErrorHandlerList { get { return new PanelChangeCommand(PanelView.ErrorHandlerList, RequestPanelChange); } }
        public static ICommand GoToAssemblyList { get { return new PanelChangeCommand(PanelView.AssemblyList, RequestPanelChange); } }
        public static ICommand GoToStartStop { get { return new PanelChangeCommand(PanelView.StartStop, RequestPanelChange); } }
        public static ICommand GoToConfigurationEdit { get { return new PanelChangeCommand(PanelView.ConfigurationEdit, RequestPanelChange); } }
        public static ICommand GoToMachineEdit { get { return new PanelChangeCommand(PanelView.MachineEdit, RequestPanelChange); } }
        public static ICommand GoToWorkerEdit { get { return new PanelChangeCommand(PanelView.WorkerEdit, RequestPanelChange); } }
        public static ICommand GoToErrorHandlerEdit { get { return new PanelChangeCommand(PanelView.ErrorHandlerEdit, RequestPanelChange); } }
        public static ICommand GoToAssemblyEdit { get { return new PanelChangeCommand(PanelView.AssemblyEdit, RequestPanelChange); } }
        public static ICommand GoToPreviousView { get { return new PanelChangeCommand(PreviousView, RequestPanelChange); } }

        public static event Action<PanelView, object> PanelChangeNeeded;

        internal static void RequestPanelChange(PanelView newView, object parameter)
        {
            if (PanelChangeNeeded != null)
            {
                PreviousView = CurrentView;
                CurrentView = newView;
                PanelChangeNeeded(newView, parameter);
            }
        }
    }
}
