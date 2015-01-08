using Informagator.ProdProviders.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Informagator.Manager.Commands
{
    public class ChangeActiveSystemConfigurationCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        #pragma warning disable 67
        public event EventHandler CanExecuteChanged = null;
        #pragma warning restore 67

        public void Execute(object parameter)
        {
            SystemConfiguration newActiveConfiguration;
            Action<SystemConfiguration> deactivate = delegate(SystemConfiguration config) { config.IsActive = false; config.EffectiveDttm = null; };
            Func<SystemConfiguration, SystemConfiguration> activate = delegate(SystemConfiguration config) { config.IsActive = true; config.EffectiveDttm = DateTime.Now; return config;};

            using (ConfigurationEntities ent = new ConfigurationEntities())
            {
                ent.SystemConfigurations.Where(c => c.IsActive).ToList().ForEach(deactivate);
                newActiveConfiguration = activate(ent.SystemConfigurations.Single(c => c.Id == (long)parameter));
                ent.SaveChanges();

                (App.Current as Manager.App).NotifyActiveSystemConfigurationChanged();
            }
        }
    }
}
