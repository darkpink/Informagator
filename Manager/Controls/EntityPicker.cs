using Informagator.DBEntities.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Informagator.Manager.Controls
{
    public abstract class EntityPicker<T> : Control
    {
        public EntityPicker()
        {
            Entities = new ObservableCollection<T>();
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                RefreshEntities();
                ConfigurationSelection.SelectedConfigurationChanged += RefreshEntities;
            }
        }


        public static readonly DependencyProperty EntityIdProperty = DependencyProperty.Register("EntityId", typeof(long?), typeof(EntityPicker<T>), new FrameworkPropertyMetadata(new PropertyChangedCallback(EntityIdChanged)) { BindsTwoWayByDefault = true });
        public long? EntityId
        {
            get
            {
                return (long?)GetValue(EntityIdProperty);
            }
            set
            {
                SetValue(EntityIdProperty, value);
            }
        }
        public static void EntityIdChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var picker = sender as EntityPicker<T>;
            if (picker != null)
            {
                picker.OnEntityIdChanged();
            }
        }

        protected void OnEntityIdChanged()
        {
        }

        public static readonly DependencyProperty EntitiesProperty = DependencyProperty.Register("Entities", typeof(ObservableCollection<T>), typeof(EntityPicker<T>), new FrameworkPropertyMetadata(new PropertyChangedCallback(EntitiesChanged)) { BindsTwoWayByDefault = true });
        public ObservableCollection<T> Entities
        {
            get
            {
                return (ObservableCollection<T>)GetValue(EntitiesProperty);
            }
            set
            {
                SetValue(EntitiesProperty, value);
            }
        }
        public static void EntitiesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var picker = sender as EntityPicker<T>;
            if (picker != null)
            {
                picker.OnEntitiesChanged();
            }
        }

        protected void OnEntitiesChanged()
        {
        }

        protected void RefreshEntities()
        {
            IEnumerable<T> entities = GetEntities();
            Entities.ToList().ForEach(m => Entities.Remove(m));
            entities.ToList().ForEach(m => Entities.Add(m));
        }
        protected abstract IEnumerable<T> GetEntities();
    }
}
