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
    public class MachinePicker : ComboBox
    {
        static MachinePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MachinePicker), new FrameworkPropertyMetadata(typeof(ComboBox)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            IsEditable = false;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                RefreshMachines();
                (App.Current as Manager.App).ActiveSystemConfigurationChanged += RefreshMachines;
            }
        }

        private void RefreshMachines()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                ItemsSource = entities.SystemConfigurations
                                      .Include(conf => conf.Machines)          
                                      .Single(conf => conf.IsActive)
                                      .Machines
                                      .Select(asc => asc.Name)
                                      .Distinct()
                                      .OrderBy(a => a);
            }
        }
    }
}
