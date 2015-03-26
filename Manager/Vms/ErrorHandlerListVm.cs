using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data.Entity;
using Informagator.Manager.Commands;

namespace Informagator.Manager.Vms
{
    public class ErrorHandlerListVm : ListPanelVmBase<ErrorHandler>
    {
        public ICommand DeleteErrorHandler
        {
            get
            {
                return new DeleteEntityCommand<ErrorHandler>(entities => entities.ErrorHandlers, (ErrorHandlers, id) => ErrorHandlers.Single(w => w.Id == (long)id), null);
            }
        }

        protected override ErrorHandler[] GetEntities()
        {
            ErrorHandler[] result;

            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                result = entities.ErrorHandlers.Include(t => t.ErrorHandlerParameters).Where(w => w.SystemConfiguration.Description == ConfigurationSelection.SelectedConfiguration).ToArray();
            }

            return result;
        }
    }
}
