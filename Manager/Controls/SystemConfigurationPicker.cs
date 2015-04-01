using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Informagator.Manager.Controls
{
    public class SystemConfigurationPicker : Control
    {
        static SystemConfigurationPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SystemConfigurationPicker), new FrameworkPropertyMetadata(typeof(SystemConfigurationPicker)));
            FocusableProperty.OverrideMetadata(typeof(SystemConfigurationPicker), new FrameworkPropertyMetadata(false));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Configurations = new ObservableCollection<string>();
            RefreshSystemConfigurations();
            
            if (!DesignerProperties.GetIsInDesignMode(this))
            ConfigurationSelection.ActiveConfigurationChanged += RefreshSystemConfigurations;
            ConfigurationSelection.SelectedConfigurationChanged += RefreshSystemConfigurations;
            ConfigurationSelection.ActiveConfigurationChanged += CheckIfSelectedConfigurationIsActive;
            ConfigurationSelection.SelectedConfigurationChanged += CheckIfSelectedConfigurationIsActive;

            CheckIfSelectedConfigurationIsActive();
        }

        private void RefreshSystemConfigurations()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                using (ConfigurationEntities entities = new ConfigurationEntities())
                {
                    List<string> newConfigs = entities.SystemConfigurations.OrderByDescending(c => c.IsActive).ThenBy(c => c.Name).Select(c => c.Name).ToList();
                    foreach(string deleted in Configurations.Except(newConfigs).ToList())
                    {
                        Configurations.Remove(deleted);
                    }

                    foreach(string added in newConfigs.Except(Configurations).ToList())
                    {
                        int index = Configurations.Any(c => String.Compare(added, c, true) > 0) ?
                            Enumerable.Range(0, Configurations.Count).First(i => String.Compare(added, Configurations[i], true) >= 0) :
                            Configurations.Count;
                        Configurations.Insert(index, added);
                    }

                    if (SelectedConfiguration == null)
                    {
                        SelectedConfiguration = entities.SystemConfigurations.Where(c => c.IsActive).Select(c => c.Name).SingleOrDefault();
                    }

                    if (SelectedConfiguration == null)
                    {
                        SelectedConfiguration = entities.SystemConfigurations.Select(c => c.Name).FirstOrDefault();
                    }
                }
            }
        }

        public static DependencyProperty ConfigurationsProperty = DependencyProperty.Register("Configurations", typeof(ObservableCollection<string>), typeof(SystemConfigurationPicker), new PropertyMetadata(new PropertyChangedCallback(ConfigurationsChanged)));

        public ObservableCollection<string> Configurations
        {
            get
            {
                return (ObservableCollection<string>)GetValue(ConfigurationsProperty);
            }
            set
            {
                SetValue(ConfigurationsProperty, value);
            }
        }

        public static void ConfigurationsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SystemConfigurationPicker picker = sender as SystemConfigurationPicker;
            if (picker != null)
            {
                picker.OnConfigurationsChanged();
            }
        }

        protected virtual void OnConfigurationsChanged()
        {
        }

        public static DependencyProperty SelectedConfigurationProperty = DependencyProperty.Register("SelectedConfiguration", typeof(string), typeof(SystemConfigurationPicker), new PropertyMetadata(new PropertyChangedCallback(SelectedConfigurationChanged)));

        public string SelectedConfiguration
        {
            get
            {
                return (string)GetValue(SelectedConfigurationProperty);
            }
            set
            {
                SetValue(SelectedConfigurationProperty, value);
            }
        }

        public static void SelectedConfigurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SystemConfigurationPicker picker = sender as SystemConfigurationPicker;
            if (picker != null)
            {
                picker.OnSelectedConfigurationChanged();
            }
        }

        protected virtual void OnSelectedConfigurationChanged()
        {
            if (SelectedConfiguration != ConfigurationSelection.SelectedConfiguration)
            {
                ConfigurationSelection.SelectedConfiguration = SelectedConfiguration;
            }
        }


        public static DependencyProperty IsSelectedConfigurationActiveProperty = DependencyProperty.Register("IsSelectedConfigurationActive", typeof(bool), typeof(SystemConfigurationPicker), new PropertyMetadata(new PropertyChangedCallback(IsSelectedConfigurationActiveChanged)));

        public bool IsSelectedConfigurationActive
        {
            get
            {
                return (bool)GetValue(IsSelectedConfigurationActiveProperty);
            }
            set
            {
                SetValue(IsSelectedConfigurationActiveProperty, value);
            }
        }

        public static void IsSelectedConfigurationActiveChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SystemConfigurationPicker picker = sender as SystemConfigurationPicker;
            if (picker != null)
            {
                picker.OnIsSelectedConfigurationActiveChanged();
            }
        }

        protected virtual void OnIsSelectedConfigurationActiveChanged()
        {
        }

        private void CheckIfSelectedConfigurationIsActive()
        {
            IsSelectedConfigurationActive = ConfigurationSelection.SelectedConfiguration == ConfigurationSelection.ActiveConfiguration;
        }
    }
}
