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
    public class MachinePicker : ComboBox
    {
        //TODO: don't derive Combobox
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
                ConfigurationSelection.SelectedConfigurationChanged += RefreshMachines;
            }
        }


        public static readonly DependencyProperty MachineIdProperty = DependencyProperty.Register("MachineId", typeof(long?), typeof(MachinePicker), new FrameworkPropertyMetadata(new PropertyChangedCallback(MachineIdChanged)) { BindsTwoWayByDefault = true });
        public long? MachineId
        {
            get
            {
                return (long?)GetValue(MachineIdProperty);
            }
            set
            {
                SetValue(MachineIdProperty, value);
            }
        }
        public static void MachineIdChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var picker = sender as MachinePicker;
            if (picker != null)
            {
                picker.OnMachineIdChanged();
            }
        }

        private void OnMachineIdChanged()
        {
            SetSelectedItem();
        }

        private void SetSelectedItem()
        {
            var source = (ItemsSource as IEnumerable<Machine>);
            Machine selectedItem;
            if (source != null)
            {
                selectedItem = source.SingleOrDefault(m => m.Id == MachineId);
            }
            else
            {
                selectedItem = null;
            }

            if (selectedItem != SelectedItem)
            {
                SelectedItem = selectedItem;
            }
        }

        private void RefreshMachines()
        {
            using (ConfigurationEntities entities = new ConfigurationEntities())
            {
                ItemsSource = entities.SystemConfigurations
                                      .Include(conf => conf.Machines)
                                      .Single(conf => conf.Description == ConfigurationSelection.SelectedConfiguration)
                                      .Machines
                                      .OrderBy(a => a.Name)
                                      .ToArray();

            }

            SetSelectedItem();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            var selected = e.AddedItems.OfType<Machine>().SingleOrDefault();
            var machineId = selected == null ? (long?)null : selected.Id;
            if (machineId != MachineId)
            {
                MachineId = machineId;
            }
        }
    }
}
