﻿using System;
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
using Rainbow.Utility;
using Rainbow.Utility.Model;
using System.Collections.ObjectModel;
using Rainbow.Utility.Communication;
using System.Data;
using Rainbow.Utility.Interface;
using Rainbow.Utility.Functions;

namespace Rainbow.MeasureUserCtrl
{
    /// <summary>
    /// UserCheckLight.xaml 的交互逻辑
    /// </summary>
    public partial class UserCheckLight : UserControl, IMeasureUser
    {
        public UserCheckLight()
        {
            InitializeComponent();
        }
        ObservableCollection<CheckLightModel> mDataList = new ObservableCollection<CheckLightModel>();
        ObservableCollection<double> mSetWaveLength_LightList = new ObservableCollection<double>();//设置的波长光强值记录
        double mAverageLight = 0;
        double mEngineer = 0; 
        double[] mLightList = null;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {

                OmniProvider.GetParameters();
                schart.CreateCoordinate(100, 900, 0,70000);
                datWave_Light.ItemsSource = mDataList;
                mSetWaveLength_LightList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(mLightList_CollectionChanged);

                txtBoxcarWidth.Text = OmniProvider.BoxcarWidth.ToString();
                txtScansToAverage.Text = OmniProvider.ScansToAverage.ToString();
                txtIntergrationTime.Text = OmniProvider.InterationTime.ToString();
            }
            catch(Exception ex)
            { 
                Functions.LOG("check load"+ex.Message);
            }
        }

        void mLightList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (mSetWaveLength_LightList.Count > 0)
            {
                double sum = 0;
                foreach (double num in mSetWaveLength_LightList)
                {
                    sum += num;
                    mAverageLight = sum / mSetWaveLength_LightList.Count;
                }

            }
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (txtWaveLength.Text != "")
            {
                OmniProvider.ScansToAverage = txtScansToAverage.IntNumber;
                OmniProvider.BoxcarWidth = txtBoxcarWidth.IntNumber;
                OmniProvider.InterationTime = txtIntergrationTime.IntNumber;

                mLightList = OmniProvider.GetSpectrum();
                schart.ShowData(OmniProvider.GetWavelengths(100,900), mLightList, 100, 200, 900, 10000, true);

                txtCurrentLight.Text = OmniProvider.GetMeasureLight(txtWaveLength.DoubleNumber, mLightList).ToString("f2");
                mSetWaveLength_LightList.Add(txtCurrentLight.DoubleNumber);
                txtAverageLight.Text = mAverageLight.ToString("f2");

                mDataList.Add(new CheckLightModel()
                {
                    WaveLength = txtWaveLength.DoubleNumber,
                    CurrentLight = txtCurrentLight.DoubleNumber,
                    AverageLight = txtAverageLight.DoubleNumber,
                    ScansToAverage = txtScansToAverage.DoubleNumber,
                    BoxcarWidth = txtBoxcarWidth.DoubleNumber,
                    IntergrationTime = txtIntergrationTime.DoubleNumber,
                    A230 = OmniProvider.GetMeasureLight(230, mLightList),
                    A260 = OmniProvider.GetMeasureLight(260, mLightList),
                    A280 = OmniProvider.GetMeasureLight(280, mLightList),
                    A290 = OmniProvider.GetMeasureLight(290, mLightList),
                    A350 = OmniProvider.GetMeasureLight(350, mLightList),
                    A500 = OmniProvider.GetMeasureLight(500, mLightList),
                    A750 = OmniProvider.GetMeasureLight(750, mLightList)
                });

            }
            else
            {
                MessageBox.Show("no wavelength");
            }
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


        }
   
        private void schart_ChangeSetWaveEvent(Point pointValue)
        {
            txtWaveLength.Text = pointValue.X.ToString();
            if(mLightList!=null)
            {
                txtCurrentLight.Text = OmniProvider.GetMeasureLight(280, mLightList).ToString(GlobalProvider.FormatNumber);
                txtAverageLight.Text = mAverageLight.ToString("f2") ;
            }
        }
    }
}
