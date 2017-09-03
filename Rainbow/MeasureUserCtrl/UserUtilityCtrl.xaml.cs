using Rainbow.Utility.Communication;
using Rainbow.Utility.Interface;
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
    /// UserUtilityCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class UserUtilityCtrl : UserControl, IMeasureUser
    {
        public UserUtilityCtrl()
        {
            InitializeComponent();
        }

        public void Blank()
        {
            throw new NotImplementedException();
        }

        public void ExportTxt()
        {
            throw new NotImplementedException();
        }

        public void Measure()
        {
            throw new NotImplementedException();
        }
        #region 调试电机
      
        
        private void Mark_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Content.ToString())
            {
                case "SaveMark1":
                    OmniProvider.SaveMark(1, txtMark1.IntNumber);
                    break;
                case "SaveMark2":
                    OmniProvider.SaveMark(2, txtMark2.IntNumber);
                    break;
                case "SaveMark3":
                    OmniProvider.SaveMark(3, txtMark3.IntNumber);
                    break;
            }
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtMark1.Text = OmniProvider.GetMark(1).ToString();
            txtMark2.Text = OmniProvider.GetMark(2).ToString();
            txtMark3.Text = OmniProvider.GetMark(3).ToString();


            txtBoxcarWidth.Text = OmniProvider.BoxcarWidth.ToString();
            txtScansToAverage.Text = OmniProvider.ScansToAverage.ToString();
            txtIntergrationTime.Text = OmniProvider.InterationTime.ToString();
        }
        //保存参数
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            OmniProvider.SetParameters(txtScansToAverage.IntNumber, txtBoxcarWidth.IntNumber, txtIntergrationTime.IntNumber);
        }
    }
}
