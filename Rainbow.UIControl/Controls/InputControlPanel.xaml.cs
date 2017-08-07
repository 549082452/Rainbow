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
using System.Collections.ObjectModel;

namespace ClipperBaseUI.Controls
{
    /// <summary>
    /// InputControlPanel.xaml 的交互逻辑
    /// </summary>
    public partial class InputControlPanel : UserControl
    {
        public delegate bool? InputEvent();

        public event InputEvent FirstEvent;
        public event InputEvent PreEvent;
        public event InputEvent NextEvent;
        public event InputEvent LastEvent;
        public event InputEvent AddEvent;
        public event InputEvent DeleteEvent;
        public event InputEvent ModifyEvent;
        public event Action RefreshEvent;

        public DataGrid CtrlDataGrid
        {
            get;
            set;
        }


        public InputControlPanel()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnfirst_Click(object sender, RoutedEventArgs e)
        {
            if (FirstEvent != null)
            {
                FirstEvent.Invoke();
            }
        }

        private void btnPre_Click(object sender, RoutedEventArgs e)
        {
            if (PreEvent != null)
            {
                PreEvent.Invoke();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (NextEvent != null)
            {
                NextEvent.Invoke();
            }
        }
        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (LastEvent != null)
            {
                LastEvent.Invoke();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddEvent != null)
            {
                bool? result = AddEvent.Invoke();
                if (result!=null&&result.Value)
                {
                    if (RefreshEvent != null)
                    {
                        RefreshEvent.Invoke();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteEvent != null)
            {
                bool? result = DeleteEvent.Invoke();
                if (result!=null&&result.Value)
                {
                    if (RefreshEvent != null)
                    {
                        RefreshEvent.Invoke();
                    }
                }
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (ModifyEvent != null)
            {
                bool? result=ModifyEvent.Invoke();
                if (result!=null&&result.Value)
                {
                    if (RefreshEvent != null)
                    {
                        RefreshEvent.Invoke();
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (RefreshEvent != null)
            {
                RefreshEvent.Invoke();
            }
        }


    }
}
