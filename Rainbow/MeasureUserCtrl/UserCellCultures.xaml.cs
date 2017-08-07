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
    /// UserCellCultures.xaml 的交互逻辑
    /// </summary>
    public partial class UserCellCultures : UserControl, IMeasureUser
    {
        public UserCellCultures()
        {
            InitializeComponent();
        }
        int mSampleTimes = 0;
        private void txtSampleID_TextChanged(object sender, TextChangedEventArgs e)
        {
            mSampleTimes = 0;
        }

        public void Blank()
        {
            throw new NotImplementedException();
        }

        public void Measure()
        {
            throw new NotImplementedException();
        }

        public void ExportTxt()
        {
            throw new NotImplementedException();
        }
    }
}
