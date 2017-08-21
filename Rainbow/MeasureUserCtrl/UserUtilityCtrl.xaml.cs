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
    }
}
