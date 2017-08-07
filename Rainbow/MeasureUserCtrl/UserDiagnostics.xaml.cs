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
using System.Collections.ObjectModel;
using Rainbow.Utility.Model;
using Rainbow.Utility.Communication;
using Rainbow.Utility.Interface;
using Rainbow.Utility;

namespace Rainbow.MeasureUserCtrl
{
    /// <summary>
    /// UserDiagnostics.xaml 的交互逻辑
    /// </summary>
    public partial class UserDiagnostics : UserControl,IMeasureUser
    {
        public UserDiagnostics()
        {
            InitializeComponent();
        }
         
        ObservableCollection<double> mQCData = new ObservableCollection<double>();
        double mVariance;//方差
        double mSD;//标准差
        double mAverage;//均值
        double mCV;//变异系数
        double mMax,mMin;//最大值,最小值
        double mErrorMax;//最大偏差

        string mFormatDouble;
        MoveEnum mGPIOMove;
        ObservableCollection<DiagnosticsModel> mDataList = new ObservableCollection<DiagnosticsModel>();

        int mEngineer = 0;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            schart.CreateCoordinate(0, 800, 0, 1);
            mDataList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(mDataList_CollectionChanged);
            dataDia.ItemsSource = mDataList;
            mFormatDouble = GlobalProvider.FormatNumber;
            mGPIOMove = MoveEnum.Move0_5mm;
        }

        void mDataList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (mQCData != null)
            {
                CalcSD(mQCData);
            }
        }
        /// <summary>
        /// 计算点中Y值的标准差
        /// </summary>
        /// <param name="numCollection">数据点</param>
        private void CalcSD(ObservableCollection<double> numCollection)
        {
            mAverage = numCollection.Average();
            foreach (double num in numCollection)
            {
                if (num > mMax)
                {
                    mMax = num;
                }
                if (num < mMin)
                {
                    mMin = num;
                }
                double errorNum = num - mAverage;
                if (errorNum > mErrorMax)
                {
                    mErrorMax = errorNum;
                }
                mVariance += Math.Pow(errorNum, 2);
            }
            mSD = Math.Pow(mVariance, 0.5);
            mCV = mSD / mAverage * 100;
        }

        private void btnBlank_Click(object sender, RoutedEventArgs e)
        {
            OmniProvider.GetBlankList(mGPIOMove);
            ClearData();
        }
        private void ClearData()
        {
            mQCData.Clear();
            mVariance = mSD = mAverage = mCV = mMax = mMin = mErrorMax = 0;

        }
        private void btnMeasure_Click(object sender, RoutedEventArgs e)
        {
            if (mQCData != null)
            {
                OmniProvider.GetMeasureList(mGPIOMove);
                mQCData.Add(OmniProvider.GetAbs(350, mGPIOMove));
                txtCurrent.Text = mQCData.Last().ToString(mFormatDouble);
                txtAverage.Text = mAverage.ToString(mFormatDouble);
                txtMaximum.Text = mMax.ToString(mFormatDouble);
                txtMinimum.Text = mMin.ToString(mFormatDouble);
                txtErrorMax.Text = mErrorMax.ToString(mFormatDouble);
                txtSTD.Text = mSD.ToString(mFormatDouble);

                DiagnosticsModel model = new DiagnosticsModel()
                {
                    Current=txtCurrent.DoubleNumber,
                    Average=txtAverage.DoubleNumber,
                    Maximum=txtMaximum.DoubleNumber,
                    Minimum=txtMinimum.DoubleNumber,
                    ErrorMax=txtErrorMax.DoubleNumber,
                    STD_DEV=txtSTD.DoubleNumber,
                    Target=txtTarget.DoubleNumber
                };
                mDataList.Add(model);
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
        #region 调试电机
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseButtonState.Pressed == e.RightButton)
            {
                if (mEngineer >= 3)
                {
                    Grid.SetColumnSpan(dataDia, 1);
                    gridAdjust.Visibility = Visibility.Visible;
                    txtMark1.Text = OmniProvider.GetMark(1).ToString();
                    txtMark2.Text = OmniProvider.GetMark(2).ToString();
                    txtMark3.Text = OmniProvider.GetMark(3).ToString();
                }
                else
                {
                    mEngineer++;
                }
            }
            else
            {
                Grid.SetColumnSpan(dataDia, 2);
                gridAdjust.Visibility = Visibility.Hidden;
            }
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            mEngineer = 0;
        }

        private void  Mark_Click(object sender, RoutedEventArgs e)
        {
            Button btn=sender as Button;
            switch(btn.Content.ToString())
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
    }
}
