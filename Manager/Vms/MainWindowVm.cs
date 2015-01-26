using Informagator.Manager.Commands;
using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Vms
{
    public class MainWindowVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private VmBase _panelVm;
        public VmBase PanelVm
        {
            get
            {
                return _panelVm;
            }
            set
            {
                _panelVm = value;
                NotifyPropertyChanged("PanelVm");
            }
        }

        private string _activeConfiguration;
        public string ActiveConfiguration 
        {
            get
            {
                return _activeConfiguration;
            }
            set
            {
                _activeConfiguration = value;
                NotifyPropertyChanged("ActiveConfiguration");
            }
        }

        protected Dictionary<PanelChangeCommandManager.PanelView, VmBase> ModePanelMap { get; set; }
        public MainWindowVm()
        {
            GetCurrentConfiguration();

            ModePanelMap = new Dictionary<PanelChangeCommandManager.PanelView, VmBase>();
            ModePanelMap.Add(PanelChangeCommandManager.PanelView.AssemblyList, new AssemblyListVm());
            ModePanelMap.Add(PanelChangeCommandManager.PanelView.ConfigurationList, new ConfigurationListVm() { SelectedConfiguration = ActiveConfiguration });
            ModePanelMap.Add(PanelChangeCommandManager.PanelView.MachineList, new MachineListVm());
            ModePanelMap.Add(PanelChangeCommandManager.PanelView.StartStop, new StartStopVm());
            ModePanelMap.Add(PanelChangeCommandManager.PanelView.WorkerList, new WorkerListVm());
            ModePanelMap.Add(PanelChangeCommandManager.PanelView.WorkerEdit, new WorkerEditVm());
            ModePanelMap.Add(PanelChangeCommandManager.PanelView.AssemblyEdit, new AssemblyEditVm());
            ModePanelMap.Add(PanelChangeCommandManager.PanelView.MachineEdit, new MachineEditVm());
            ModePanelMap.Add(PanelChangeCommandManager.PanelView.ConfigurationEdit, new ConfigurationEditVm());

            (App.Current as App).ActiveSystemConfigurationChanged += GetCurrentConfiguration;
            PanelVm = ModePanelMap[PanelChangeCommandManager.PanelView.StartStop];

            PanelChangeCommandManager.PanelChangeNeeded += PanelChangeCommandManager_PanelChangeNeeded;
        }

        private void PanelChangeCommandManager_PanelChangeNeeded(PanelChangeCommandManager.PanelView view, object parameter)
        {
            PanelVm = ModePanelMap[view];
            PanelVm.Parameter = parameter;
            PanelVm.Refresh();
        }

        private void GetCurrentConfiguration()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                ActiveConfiguration = entities.SystemConfigurations.Where(c => c.IsActive).Select(c => c.Description).SingleOrDefault();
            }
        }
        
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
