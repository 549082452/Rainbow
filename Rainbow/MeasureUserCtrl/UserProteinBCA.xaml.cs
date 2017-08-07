using Rainbow.Utility.Communication;
using Rainbow.Utility.Interface;
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
    /// UserProteinBCA.xaml 的交互逻辑
    /// </summary>
    public partial class UserProteinBCA : UserControl, IMeasureUser
    {
        public UserProteinBCA()
        {
            InitializeComponent();
        }

        public UserProteinBCA(ProteinTypeEnum type)
        {
            InitializeComponent();
            switch(type)
            {
                case ProteinTypeEnum.BCA:
                    tbkAbsWaveAtWavelength.Text = "Absorbance at 562 nm";
                    break;
                case ProteinTypeEnum.Bradford:
                    tbkAbsWaveAtWavelength.Text = "Absorbance at 595 nm";
                    break;
                case ProteinTypeEnum.Lowry:
                    tbkAbsWaveAtWavelength.Text = "Absorbance at 650 nm";
                    break;
                case ProteinTypeEnum.Pierce_660_nm:
                    tbkAbsWaveAtWavelength.Text = "Absorbance at 660 nm";
                    break;
            }
        }

        int mSampleTimes = 0;

        private void txtSampleID_TextChanged(object sender, TextChangedEventArgs e)
        {
            mSampleTimes = 0;
        }

        public void Blank()
        {
            OmniProvider.GetBlankList(MoveEnum.Move0_5mm);
        }

        public void Measure()
        {
            if(rbdSample.IsChecked.Value)
            {
                
            }
            else if(rbdStandard.IsChecked.Value)
            {

            }
        }

        public void ExportTxt()
        {
            throw new NotImplementedException();
        }
        
    }
}
