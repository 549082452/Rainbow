using Rainbow.Utility;
using Rainbow.Utility.Interface;
using Rainbow.Utility.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// UserMicroArray.xaml 的交互逻辑
    /// </summary>
    public partial class UserMicroArray : UserControl, IMeasureUser
    {
        public UserMicroArray()
        {
            InitializeComponent();
        }
      
        private void txtSampleID_TextChanged(object sender, TextChangedEventArgs e)
        {
            mSampleTimes = 0;
        }
        private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtTypeValue.Text = cboType.SelectedValue.ToString();
            if (txtTypeValue.Text == "0")
            {
                txtTypeValue.IsEnabled = true;
            }
            else
            {
                txtTypeValue.IsEnabled = false;
            }
        }

        public void Blank()
        {
            
        }

        public void Measure()
        {
            
        }

        public void ExportTxt()
        {
            
        }

        int mSampleTimes = 0;
        ObservableCollection<MeasureDataModel> DataList = new ObservableCollection<MeasureDataModel>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            schart.CreateCoordinate(200, 750, 0, 10);
            List<NucleicAcidTypeModel> typeList = new List<NucleicAcidTypeModel>();
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "DsDNA", TypeValue = 33 });
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "RNA", TypeValue = 40 });
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "ssDNA", TypeValue = 50 });
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "Custom", TypeValue = 1 });
            cboType.ItemsSource = typeList;
            
            dataGrid.ItemsSource = DataList;

            cboDye1.ItemsSource = GlobalProvider.DyeList;
            cboDye2.ItemsSource = GlobalProvider.DyeList;
        }
    }
}
