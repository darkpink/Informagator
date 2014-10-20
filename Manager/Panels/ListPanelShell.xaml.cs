using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;

namespace Acadian.Informagator.Manager.Panels
{
    /// <summary>
    /// Interaction logic for ListBase.xaml
    /// </summary>
    public partial class ListPanelShell : UserControl
    {
        public ListPanelShell()
        {
            InitializeComponent();
        }

        public virtual string ScreenTitle
        {
            get
            {
                throw new InvalidOperationException("You must override the ScreenTitle property");
            }
        }

        public virtual string EntityName
        {
            get
            {
                throw new InvalidOperationException("You must override the EntityName property");
            }
        }

        private RoutedUICommand _newCommand;
        public RoutedUICommand NewCommand
        {
            get
            {
                return _newCommand;
            }
            set
            {
                _newCommand = value;
            }
        }

        protected ManagementItemCache DefinitionCache
        {
            get
            {
                ManagementItemCache result = App.ManagementItemCache;
                return result;
            }
        }

        protected virtual void uxFirstPage_Click(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void uxPreviousPage_Click(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void uxNextPage_Click(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void uxLastPage_Click(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void uxShowAll_Click(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void uxShowPages_Click(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void uxGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
        }

        protected virtual void uxGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
        }

        public ObservableCollection<DataGridColumn> Columns
        {
            get
            {
                //return uxGrid.Columns;
                return null;
            }
        }


        protected virtual string EditSecurity
        {
            get
            {
                return null;
            }
        }
    }
}
