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
using System.Collections.ObjectModel;

namespace Informagator.Manager.Controls
{
    public class MachinePicker : EntityPicker<Machine>
    {
        static MachinePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MachinePicker), new FrameworkPropertyMetadata(typeof(MachinePicker)));
            FocusableProperty.OverrideMetadata(typeof(MachinePicker), new FrameworkPropertyMetadata(false));
        }

        public MachinePicker()
            :base()
        {
        }

        protected override IEnumerable<Machine> GetEntities()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                var machines = entities.SystemConfigurations
                                      .Include(conf => conf.Machines)
                                      .Single(conf => conf.Name == ConfigurationSelection.SelectedConfiguration)
                                      .Machines
                                      .ToArray();
                return machines;
            }
        }
    }
}
