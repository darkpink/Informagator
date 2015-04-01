using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace Informagator.Manager.Controls
{
    public class AssemblyPicker : ComboBox
    {
        static AssemblyPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AssemblyPicker), new FrameworkPropertyMetadata(typeof(ComboBox)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            IsEditable = false;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                RefreshAssemblies();
                ConfigurationSelection.SelectedConfigurationChanged += RefreshAssemblies;
            }
        }

        private void RefreshAssemblies()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                ItemsSource = entities.SystemConfigurations
                                      .Include(conf => conf.Assemblies)          
                                      .Single(conf => conf.Name == ConfigurationSelection.SelectedConfiguration)
                                      .Assemblies
                                      .OrderBy(a => a.ToString());
            }
        }
    }
}
