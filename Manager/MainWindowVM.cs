using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.Manager
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ManagementItemCache _itemCache;

        private bool _isEditable;
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }
            set
            {
                _isEditable = value;
                NotifyPropertyChanged("IsEditable");
            }
        }

        private long _applicationVersion;
        public long ApplicationVersion 
        {
            get
            {
                return _applicationVersion;
            }
            set
            {
                _applicationVersion = value;
                NotifyPropertyChanged("ApplicationVersion");
            }
        }

        public ManagementItemCache ItemCache
        {
            get
            {
                return _itemCache;
            }
            set
            {
                _itemCache = value;
                NotifyPropertyChanged("ItemCache");
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
