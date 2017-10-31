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
using Rainbow.Utility.Communication;
using Rainbow.Utility.Interface;
using Rainbow.Utility.Model;
using System.Collections.ObjectModel;
using Rainbow.Utility;
using System.Threading;
using Rainbow.Utility.Functions;

namespace Rainbow.MeasureUserCtrl
{
    /// <summary>
    /// UserNucleicAcid.xaml 的交互逻辑
    /// </summary>
    public partial class UserNucleicAcid : UserControl,IMeasureUser
    {
        int mSampleTimes = 0;
        ObservableCollection<MeasureDataModel> DataList = new ObservableCollection<MeasureDataModel>();

        public UserNucleicAcid()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            schart.CreateCoordinate(220, 360, 0, 10);
            schart.SetWave(double.Parse(txtSW.Text));
            List<NucleicAcidTypeModel> typeList = new List<NucleicAcidTypeModel>();
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "DsDNA", TypeValue = 33 });
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "RNA", TypeValue = 40 });
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "ssDNA", TypeValue = 50 });
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "Custom", TypeValue = 1 });
            cboType.ItemsSource = typeList;
            dataGrid.ItemsSource = DataList;
        }


        private void txtSW_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSW.Text != "" && txtSwAbs != null)
            {
                double bestWaveSet = OmniProvider.GetBestWaveLength(double.Parse(txtSW.Text));
                if (bestWaveSet > 0)
                {
                    txtSwAbs.Text = OmniProvider.GetAbs(bestWaveSet, GlobalProvider.CurrentMove).ToString("f2");
                    schart.SetWave(double.Parse(txtSW.Text));
                }
            }
        }
        private void schart_ChangeSetWaveEvent(Point pointValue)
        {
            txtSW.Text = pointValue.X.ToString("f2");
        }


        public void Blank()
        {
            OmniProvider.Reset();
            OmniProvider.GetBlankList(MoveEnum.Move0_5mm);
            OmniProvider.GetBlankList(MoveEnum.Move0_2mm);
        }

        public void Measure()
        {
            double k = 2;
            double b = txtTypeValue.DoubleNumber;
            double abs260 = 0;
            double abs280 = 0;
            GlobalProvider.CurrentMove = MoveEnum.Move0_5mm;
            GlobalProvider.MarkOrder = 0;

            OmniProvider.GetMeasureList(GlobalProvider.CurrentMove);

           
            schart.ShowData(OmniProvider.GetWavelengths(220,360), OmniProvider.CalcY(GlobalProvider.CurrentMove), 220, 0, 360, 2, true);


            mSampleTimes++;
            txtTimes.Text = mSampleTimes.ToString();
            abs260 = OmniProvider.GetAbs(260, GlobalProvider.CurrentMove);

            if (abs260 < 2)
            {
                txtConc.Text = (Math.Log10(abs260) * k * b).ToString();
            }
            else
            {
                GlobalProvider.CurrentMove = MoveEnum.Move0_2mm;
                GlobalProvider.MarkOrder = 1;
                k = 5;
                OmniProvider.GetMeasureList(GlobalProvider.CurrentMove);
                abs260 = OmniProvider.GetAbs(260, GlobalProvider.CurrentMove);
                txtConc.Text = (Math.Log10(abs260) * k * b ).ToString();
            }
           txtSwAbs.Text = OmniProvider.GetAbs(260, GlobalProvider.CurrentMove).ToString("f2");
           abs260 = OmniProvider.GetAbs(260, GlobalProvider.CurrentMove);
           abs280 = OmniProvider.GetAbs(280, GlobalProvider.CurrentMove);
            txt260Abs.Text = abs260.ToString("f2");
            txt280Abs.Text = abs280.ToString("f2");
            txt260_280.Text = (abs260 / abs280).ToString("f2");
            txt260_230.Text = (abs260 / OmniProvider.GetAbs(230, GlobalProvider.CurrentMove)).ToString("f2");
          
            MeasureDataModel model = new MeasureDataModel()
            {
                SampleID=txtSampleID.Text,
                SampleTestTimes=mSampleTimes,
                SWnm=double.Parse(txtSW.Text),
                A260 = OmniProvider.GetAbs(260, GlobalProvider.CurrentMove),
                A230 = OmniProvider.GetAbs(230, GlobalProvider.CurrentMove),
                A280 = OmniProvider.GetAbs(280, GlobalProvider.CurrentMove),
                BlankList = OmniProvider.BlankList[GlobalProvider.MarkOrder],
                MeasureList = OmniProvider.MeasureList[GlobalProvider.MarkOrder],
                Conc=double.Parse(txtConc.Text),
                WaveList = OmniProvider.WaveList
            };
            DataList.Add(model);
        }
      
        public void ExportTxt()
        {
            Functions.ExportDataGrid(dataGrid);
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
    }
}
