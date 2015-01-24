using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.SystemStatus
{
    public class AutoRefreshingThreadStatus : INotifyPropertyChanged
    {
        private string _machineName;
        private string _threadName;
        private ThreadRunStatus _statusCode;
        private string _info;

        public string MachineName
        {
            get
            {
                return _machineName;
            }
            set
            {
                _machineName = value;
                NotifyPropertyChanged("MachineName");
            }
        }
        public string ThreadName
        {
            get
            {
                return _threadName;
            }
            set
            {
                _threadName = value;
                NotifyPropertyChanged("ThreadName");
            }
        }
        public ThreadRunStatus StatusCode
        {
            get
            {
                return _statusCode;
            }
            set
            {
                _statusCode = value;
                NotifyPropertyChanged("StatusCode");
            }
        }
        public string Info
        {
            get
            {
                return _info;
            }
            set
            {
                _info = value;
                NotifyPropertyChanged("Info");
            }
        }

        public void UpdateFromService(string serviceUrl)
        {
            var infoServiceClient = new InfoServiceClient(serviceUrl);
            IThreadStatus status = infoServiceClient.GetStatus(ThreadName);
            if (status == null)
            {
                StatusCode = ThreadRunStatus.Unknown;
                Info = "Unable to communicate";
            }
            else
            {
                StatusCode = status.RunStatus;
                Info = status.Info;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

