using Informagator.DBEntities.Configuration;
using Informagator.Manager.Vms;
using Informagator.ProdProviders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Informagator.Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App CurrentApplication
        {
            get
            {
                return (App.Current as App);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetSelectedConfiguration();

            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        private void SetSelectedConfiguration()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                ConfigurationSelection.ActiveConfiguration = entities.SystemConfigurations.Where(c => c.IsActive).Select(c => c.Description).SingleOrDefault();
                ConfigurationSelection.SelectedConfiguration = ConfigurationSelection.ActiveConfiguration;
            }
        }
    }
}
