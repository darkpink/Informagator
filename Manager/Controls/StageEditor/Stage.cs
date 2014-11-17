using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Manager.Controls.StageEditor
{
    public class Stage : INotifyPropertyChanged
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

        private string _stageAssemblyName;
        public string StageAssemblyName
        {
            get
            {
                return _stageAssemblyName;
            }
            set
            {
                _stageAssemblyName = value;
                NotifyPropertyChanged("StageAssemblyName");
            }
        }

        private string _stageAssemblyDotNetVersion;
        public string StageAssemblyDotNetVersion
        {
            get
            {
                return _stageAssemblyDotNetVersion;
            }
            set
            {
                _stageAssemblyDotNetVersion = value;
                NotifyPropertyChanged("StageAssemblyDotNetVersion");
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

        private string _errorHandlerAssemblyName;
        public string ErrorHandlerAssemblyName
        {
            get
            {
                return _errorHandlerAssemblyName;
            }
            set
            {
                _errorHandlerAssemblyName = value;
                NotifyPropertyChanged("ErrorHandlerAssemblyName");
            }
        }

        private string _errorHandlerAssemblyDotNetVersion;
        public string ErrorHandlerAssemblyDotNetVersion
        {
            get
            {
                return _errorHandlerAssemblyDotNetVersion;
            }
            set
            {
                _errorHandlerAssemblyDotNetVersion = value;
                NotifyPropertyChanged("ErrorHandlerAssemblyDotNetVersion");
            }
        }

        private string _errorHandlerType;
        public string ErrorHandlerType
        {
            get
            {
                return _errorHandlerType;
            }
            set
            {
                _errorHandlerType = value;
                NotifyPropertyChanged("ErrorHandlerType");
            }
        }

        private List<StageParameter> _parameters;
        public List<StageParameter> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
                NotifyPropertyChanged("Parameters");
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
    }
}
