using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Informagator.Manager.Controls.StageEditor
{
    public class Stage : DependencyObject, INotifyPropertyChanged
    {
        private string _name;
        public string Name
        { 
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private long? _stageAssemblyId;
        public long? StageAssemblyId
        {
            get
            {
                return _stageAssemblyId;
            }
            set
            {
                _stageAssemblyId = value;
                NotifyPropertyChanged("StageAssemblyId");
            }
        }

        private string _stageType;
        public string StageType
        {
            get
            {
                return _stageType;
            }
            set
            {
                _stageType = value;
                NotifyPropertyChanged("StageType");
            }
        }

        private ObservableCollection<StageParameter> _stageParameters;
        public ObservableCollection<StageParameter> StageParameters
        {
            get
            {
                return _stageParameters;
            }
            set
            {
                _stageParameters = value;
                NotifyPropertyChanged("StageParameters");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Stage()
        {
            StageParameters = new ObservableCollection<StageParameter>();
        }
    }
}
