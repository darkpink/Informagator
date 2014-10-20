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

namespace Acadian.Informagator.Manager.Panels
{
    public partial class EditPanelShell : UserControl
    {

        public event EventHandler SaveRequest;
        public event EventHandler CancelRequest;

        public EditPanelShell()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (((RoutedUICommand)e.Command).Name)
            {
                case "SaveEdit":
                    if (SaveRequest != null)
                    {
                        SaveRequest(this, new EventArgs());
                    }
                    break;
                case "CancelEdit":
                    if (CancelRequest != null)
                    {
                        CancelRequest(this, new EventArgs());
                    }
                    break;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _uxScroller = GetTemplateChild("uxScroller") as ScrollViewer;
            if (_uxScroller != null)
            {
                _uxScroller.PreviewMouseWheel += new MouseWheelEventHandler(uxScroller_PreviewMouseWheel);
            }
        }

        private ScrollViewer _uxScroller;

        private void uxScroller_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            double newOffset = _uxScroller.ContentVerticalOffset - e.Delta;
            _uxScroller.ScrollToVerticalOffset(newOffset);
        }
    }
}
