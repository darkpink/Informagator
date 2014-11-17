using Acadian.Informagator.Manager.Vms;
using Acadian.Informagator.ProdProviders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Acadian.Informagator.Manager
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

        public event Action ActiveSystemConfigurationChanged;

        internal void NotifyActiveSystemConfigurationChanged()
        {
            if (ActiveSystemConfigurationChanged != null)
            {
                ActiveSystemConfigurationChanged();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}
