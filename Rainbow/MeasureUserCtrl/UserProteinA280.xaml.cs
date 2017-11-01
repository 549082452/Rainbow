using Rainbow.Utility;
using Rainbow.Utility.Communication;
using Rainbow.Utility.Functions;
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
    /// UserProteinA280.xaml 的交互逻辑
    /// </summary>
    public partial class UserProteinA280 : UserControl, IMeasureUser
    {
        public UserProteinA280()
        {
            InitializeComponent();
        }
        int mSampleTimes = 0;
        ObservableCollection<MeasureDataModel> DataList = new ObservableCollection<MeasureDataModel>();

        private void txtSampleID_TextChanged(object sender, TextChangedEventArgs e)
        {
            mSampleTimes = 0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            schart.CreateCoordinate(220, 340, 0, 1);
            List<NucleicAcidTypeModel> typeList = new List<NucleicAcidTypeModel>();
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "BSA", TypeValue = 1/6.7 });
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "IGG", TypeValue = 1/13.7 });
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "LYSOZYME", TypeValue = 1/26.4 });
            typeList.Add(new NucleicAcidTypeModel() { TypeName = "1MG/1ABS", TypeValue = 1 });
            cboType.ItemsSource = typeList;
            dataGrid.ItemsSource = DataList;
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
            double b = (cboType.SelectedItem as NucleicAcidTypeModel).TypeValue;
            double abs260 = 0;
            double abs280 = 0;
            GlobalProvider.CurrentMove = MoveEnum.Move0_5mm;
            GlobalProvider.MarkOrder = 0;

            OmniProvider.GetMeasureList(GlobalProvider.CurrentMove);

            schart.ShowData(OmniProvider.GetWavelengths(220,340), OmniProvider.CalcY(GlobalProvider.CurrentMove), 220, 0, 340, 2, true);

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
                txtConc.Text = (Math.Log10(abs260) * k * b).ToString();
            }
            abs260 = OmniProvider.GetAbs(260, GlobalProvider.CurrentMove);
            abs280 = OmniProvider.GetAbs(280, GlobalProvider.CurrentMove);
            txtA280.Text = abs280.ToString("f2");
           
            txt260_280Abs.Text = (abs260 / abs280).ToString("f2");
           
            MeasureDataModel model = new MeasureDataModel()
            {
                SampleID = txtSampleID.Text,
                SampleTestTimes = mSampleTimes,
                A260 = OmniProvider.GetAbs(260, GlobalProvider.CurrentMove),
                A280 = OmniProvider.GetAbs(280, GlobalProvider.CurrentMove),
                BlankList = OmniProvider.BlankList[GlobalProvider.MarkOrder],
                MeasureList = OmniProvider.MeasureList[GlobalProvider.MarkOrder],
                Conc = double.Parse(txtConc.Text),
                WaveList = OmniProvider.WaveList
            };
            DataList.Add(model);
        }

        public void ExportTxt()
        {
            Functions.ExportDataGrid(dataGrid);
        }

    }
}
