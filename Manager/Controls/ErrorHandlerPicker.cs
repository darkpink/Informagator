using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows;

namespace Informagator.Manager.Controls
{
    public class ErrorHandlerPicker : EntityPicker<ErrorHandler>
    {
        static ErrorHandlerPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorHandlerPicker), new FrameworkPropertyMetadata(typeof(ErrorHandlerPicker)));
            FocusableProperty.OverrideMetadata(typeof(ErrorHandlerPicker), new FrameworkPropertyMetadata(false));
        }

        public ErrorHandlerPicker()
            : base()
        {
        }

        protected override IEnumerable<ErrorHandler> GetEntities()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                var ErrorHandlers = entities.SystemConfigurations
                                      .Include(conf => conf.ErrorHandlers)
                                      .Single(conf => conf.Name == ConfigurationSelection.SelectedConfiguration)
                                      .ErrorHandlers
                                      .ToArray();
                return ErrorHandlers;
            }
        }
    }
}
