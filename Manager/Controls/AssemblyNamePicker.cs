using Informagator.ProdProviders.Configuration;
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
    public class AssemblyNamePicker : ComboBox
    {
        static AssemblyNamePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AssemblyNamePicker), new FrameworkPropertyMetadata(typeof(ComboBox)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            IsEditable = false;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                RefreshAssemblies();
                (App.Current as Manager.App).ActiveSystemConfigurationChanged += RefreshAssemblies;
            }
        }

        private void RefreshAssemblies()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                ItemsSource = entities.SystemConfigurations
                                      .Include(conf => conf.AssemblySystemConfigurations)          
                                      .Single(conf => conf.IsActive)
                                      .AssemblySystemConfigurations
                                      .Select(asc => asc.AssemblyName)
                                      .Distinct()
                                      .OrderBy(a => a);
            }
        }

    }
}
