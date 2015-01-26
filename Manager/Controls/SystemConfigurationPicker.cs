using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Informagator.Manager.Controls
{
    public class SystemConfigurationPicker : ComboBox
    {
        static SystemConfigurationPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SystemConfigurationPicker), new FrameworkPropertyMetadata(typeof(ComboBox)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            IsEditable = false;

            RefreshSystemConfigurations();
            
            if (!DesignerProperties.GetIsInDesignMode(this))
            (App.Current as Manager.App).ActiveSystemConfigurationChanged += RefreshSystemConfigurations;

        }

        private void RefreshSystemConfigurations()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                using (ConfigurationEntities entities = new ConfigurationEntities())
                {
                    ItemsSource = entities.SystemConfigurations.OrderByDescending(c => c.IsActive).ThenBy(c => c.Description).Select(c => c.Description).ToList();
                }
            }
        }
    }
}
