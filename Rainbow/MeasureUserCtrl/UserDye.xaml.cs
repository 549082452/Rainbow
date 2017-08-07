using Rainbow.BusinessLogic;
using Rainbow.Utility;
using Rainbow.Utility.Model;
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

namespace Rainbow.MeasureUserCtrl
{
    /// <summary>
    /// UserData.xaml 的交互逻辑
    /// </summary>
    public partial class UserDye : UserControl
    {
        public UserDye()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            datDye.ItemsSource = GlobalProvider.DyeList;

        }

        private void datDye_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DyeModel model = datDye.SelectedItem as DyeModel;
            
        }

        private void datDye_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DyeModel model = datDye.SelectedItem as DyeModel;

        }

        private void btnSaveDye_Click(object sender, RoutedEventArgs e)
        {
            DyeListBLL.SaveInitDye();
        }
    }
}
