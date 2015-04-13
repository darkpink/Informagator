using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Informagator.Manager.Controls
{
    public class Stage : DependencyObject, INotifyPropertyChanged
    {
        private string _entityName;
        public string EntityName
        { 
            get
            {
                return _entityName;
            }
            set
            {
                _entityName = value;
                NotifyPropertyChanged("EntityName");
            }
        }

        private long? _assemblyId;
        public long? AssemblyId
        {
            get
            {
                return _assemblyId;
            }
            set
            {
                _assemblyId = value;
                NotifyPropertyChanged("AssemblyId");
            }
        }

        private string _entityType;
        public string EntityType
        {
            get
            {
                return _entityType;
            }
            set
            {
                _entityType = value;
                NotifyPropertyChanged("EntityType");
            }
        }

        private bool _suppressParentErrorHandlers;
        public bool SuppressParentErrorHandlers
        {
            get
            {
                return _suppressParentErrorHandlers;
            }
            set
            {
                _suppressParentErrorHandlers = value;
                NotifyPropertyChanged("SuppressParentErrorHandlers");
            }
        }

        private ObservableCollection<long?> _errorHandlerIds;
        public ObservableCollection<long?> ErrorHandlerIds
        {
            get
            {
                return _errorHandlerIds;
            }
            set
            {
                _errorHandlerIds = value;
                NotifyPropertyChanged("ErrorHandlerIds");
            }
        }

        private ObservableCollection<Parameter> _parameters;
        public ObservableCollection<Parameter> Parameters
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

        public Stage()
        {
            Parameters = new ObservableCollection<Parameter>();
            ErrorHandlerIds = new ObservableCollection<long?>();
            ErrorHandlerIds.CollectionChanged += ErrorHandlerIds_CollectionChanged;
        }

        void ErrorHandlerIds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }
    }
}
