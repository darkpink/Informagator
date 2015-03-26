using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.Manager
{
    public static class ConfigurationSelection
    {
        private static string _activeConfiguration;

        private static string _selectedConfiguration;

        private static string[] _configurationNames;
        
        private static bool _allowSelectedConfigurationChange;

        public static string ActiveConfiguration 
        { 
            get
            {
                return _activeConfiguration;
            }

            set
            {
                _activeConfiguration = value;
                if (ActiveConfigurationChanged != null)
                {
                    ActiveConfigurationChanged();
                }
            }
        }

        public static string SelectedConfiguration
        {
            get
            {
                return _selectedConfiguration;
            }

            set
            {
                _selectedConfiguration = value;
                if (SelectedConfigurationChanged != null)
                {
                    SelectedConfigurationChanged();
                }
            }
        }

        public static string[] ConfigurationNames
        {
            get
            {
                return _configurationNames;
            }

            set
            {
                _configurationNames = value;
                if (ConfigurationNamesChanged != null)
                {
                    ConfigurationNamesChanged();
                }
            }
        }


        public static bool AllowSelectedConfigurationChange
        {
            get
            {
                return _allowSelectedConfigurationChange;
            }

            set
            {
                _allowSelectedConfigurationChange = value;
                if (AllowSelectedConfigurationChangeChanged != null)
                {
                    AllowSelectedConfigurationChangeChanged();
                }
            }
        }


        public static event Action ActiveConfigurationChanged;

        public static event Action SelectedConfigurationChanged;

        public static event Action ConfigurationNamesChanged;

        public static event Action AllowSelectedConfigurationChangeChanged;
    }
}
