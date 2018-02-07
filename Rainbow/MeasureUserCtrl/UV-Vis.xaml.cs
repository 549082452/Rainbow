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
    /// UV_Vis.xaml 的交互逻辑
    /// </summary>
    public partial class UV_Vis : UserControl, IMeasureUser
    {
        public UV_Vis()
        {
            InitializeComponent();
        }
        int mSampleTimes = 0;
        ObservableCollection<MeasureDataModel> DataList = new ObservableCollection<MeasureDataModel>();
        ObservableCollection<Wavelength_AbsModel> mWavelength_AbsList = new ObservableCollection<Wavelength_AbsModel>();

        private void txtSampleID_TextChanged(object sender, TextChangedEventArgs e)
        {
            mSampleTimes = 0;
        }

        public void Blank()
        {
            OmniProvider.Reset();
            OmniProvider.GetBlankList(MoveEnum.Move0_5mm);
            OmniProvider.GetBlankList(MoveEnum.Move0_2mm);
        }

        public void Measure()
        {
            
            GlobalProvider.CurrentMove = MoveEnum.Move0_5mm;
            GlobalProvider.MarkOrder = 0;
            Wavelength_AbsModel wave_abs = null;
            MeasureDataModel model = null;
            OmniProvider.GetMeasureList(GlobalProvider.CurrentMove);

            schart.ShowData(OmniProvider.GetWavelengths(200,800), OmniProvider.CalcY(GlobalProvider.CurrentMove), 200, 0, 800, 2, true);

            mSampleTimes++;
            txtTimes.Text = mSampleTimes.ToString();
            if (dgdWavelength_Abs.SelectedItem != null)
            {
                wave_abs = dgdWavelength_Abs.SelectedItem as Wavelength_AbsModel;
                wave_abs.Abs= OmniProvider.GetAbs(wave_abs.Wavelength , GlobalProvider.CurrentMove);
            }
            if (wave_abs != null)
            {
                model = new MeasureDataModel()
                {
                    SampleID = txtSampleID.Text,
                    SampleTestTimes = mSampleTimes,
                    Wavelength=wave_abs.Wavelength,
                    Abs=wave_abs.Abs,
                    BlankList = OmniProvider.BlankList[GlobalProvider.MarkOrder],
                    MeasureList = OmniProvider.MeasureList[GlobalProvider.MarkOrder],
                    WaveList = OmniProvider.WaveList
                };
                DataList.Add(model);
            }

        }

        public void ExportTxt()
        {
            UIHelper.ExportDataGrid(dataGrid);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            schart.CreateCoordinate(200, 800, 0, 1);
            dgdWavelength_Abs.ItemsSource = mWavelength_AbsList;
        }

        private void btnAddWaveLength_Click(object sender, RoutedEventArgs e)
        {
            mWavelength_AbsList.Add(new Wavelength_AbsModel());
            
        }

        private void btnClearWaveLength_Click(object sender, RoutedEventArgs e)
        {
            if(dgdWavelength_Abs.SelectedItem!=null)
            {
                mWavelength_AbsList.Remove(dgdWavelength_Abs.SelectedItem as Wavelength_AbsModel);
            }
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            mWavelength_AbsList.Clear();
        }
    }
}
