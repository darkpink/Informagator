using Acadian.Informagator.Manager.Commands;
using Acadian.Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Acadian.Informagator.Manager.Vms
{
    public class MainWindowVm : INotifyPropertyChanged
    {
        public enum PanelView { ConfigurationList, MachineList, AssemblyList, WorkerList, StartStop, GlobalSettings };

        public event PropertyChangedEventHandler PropertyChanged;

        private object _panelVm;
        public object PanelVm
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

        public ICommand ChangePanelView
        {
            get
            {
                return new ParameterKeyedActionCommand(ModePanelMap);
            }
        }

        protected Dictionary<PanelView, Action> ModePanelMap { get; set; }
        public MainWindowVm()
        {
            ModePanelMap = new Dictionary<PanelView, Action>();
            ModePanelMap.Add(PanelView.AssemblyList, () => PanelVm = new AssemblyListVm());
            ModePanelMap.Add(PanelView.ConfigurationList, () => PanelVm = new ConfigurationListVm());
            ModePanelMap.Add(PanelView.GlobalSettings, () => PanelVm = new GlobalSettingsVm());
            ModePanelMap.Add(PanelView.MachineList, () => PanelVm = new MachineListVm());
            ModePanelMap.Add(PanelView.StartStop, () => PanelVm = new StartStopVm());
            ModePanelMap.Add(PanelView.WorkerList, () => PanelVm = new WorkerListVm());

            (App.Current as App).ActiveSystemConfigurationChanged += GetCurrentConfiguration;
            GetCurrentConfiguration();
            PanelVm = new ConfigurationListVm() { SelectedConfiguration = ActiveConfiguration };
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
