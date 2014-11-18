using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Acadian.Informagator.Manager.Controls.StageEditor
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

        //private string _stageAssemblyName;
        //public string StageAssemblyName
        //{
        //    get
        //    {
        //        return _stageAssemblyName;
        //    }
        //    set
        //    {
        //        _stageAssemblyName = value;
        //        NotifyPropertyChanged("StageAssemblyName");
        //    }
        //}

        public static DependencyProperty StageAssemblyNameProperty = DependencyProperty.Register("StageAssemblyName", typeof(string), typeof(Stage), new PropertyMetadata(new PropertyChangedCallback(StageAssemblyNameChanged)));
        public string StageAssemblyName
        {
            get
            {
                return (string)GetValue(StageAssemblyNameProperty);
            }
            set
            {
                SetValue(StageAssemblyNameProperty, value);
            }
        }

        public static void StageAssemblyNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
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

        private List<StageParameter> _stageParameters;
        public List<StageParameter> StageParameters
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

        private List<StageParameter> _errorHandlerParameters;
        public List<StageParameter> ErrorHandlerParameters
        {
            get
            {
                return _errorHandlerParameters;
            }
            set
            {
                _errorHandlerParameters = value;
                NotifyPropertyChanged("ErrorHandlerParameters");
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
            StageParameters = new List<StageParameter>();
            ErrorHandlerParameters = new List<StageParameter>();
        }
    }
}
