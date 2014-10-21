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
        public static ManagementItemCache ManagementItemCache { get; set;}

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ManagementItemCache = new Manager.ManagementItemCache();
            ManagementItemCache.LoadVersion(1L);
            MainWindow.DataContext = new MainWindowVM() { ItemCache = ManagementItemCache, ApplicationVersion = 1, IsEditable = false };
        }
    }
}
