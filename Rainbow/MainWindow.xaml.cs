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
using Rainbow.Utility.Functions;
using Rainbow.MeasureUserCtrl;
using Rainbow.Utility;
using Rainbow.Utility.Interface;
using Rainbow.Utility.Model;
using Rainbow.BusinessLogic;

namespace Rainbow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region 选择测试类型
        private void MeasureOperate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
             
            switch (btn.Name)
            {
                case "btnNucleicAcid":
                    GlobalProvider.MeasureUserCtrl = new UserNucleicAcid();
                    break;
                case "btnMicroArray":
                
                    GlobalProvider.MeasureUserCtrl = new UserMicroArray();
                    break;
                case "btnUV_Vis":
                    GlobalProvider.MeasureUserCtrl = new UV_Vis();
                    break;
                case "btnCellCultures":
                    GlobalProvider.MeasureUserCtrl = new UserCellCultures();
                    break;
                case "btnProteionA280":
                    GlobalProvider.MeasureUserCtrl = new UserProteinA280();
                    break;
                case "btnProteinLabels":
                    GlobalProvider.MeasureUserCtrl = new UserProteinLabel();
                    break;
                case "btnProteinBCA":
                    GlobalProvider.MeasureUserCtrl = new UserProteinBCA(ProteinTypeEnum.BCA);
                    break;
                case "btnProteinBraford":
                    GlobalProvider.MeasureUserCtrl = new UserProteinBCA(ProteinTypeEnum.Bradford);
                    break;
                case "btnProteinLowry":
                    GlobalProvider.MeasureUserCtrl = new UserProteinBCA(ProteinTypeEnum.Lowry);
                    break;
                case "btnProteinPicrce660nm":
                    GlobalProvider.MeasureUserCtrl = new UserProteinBCA(ProteinTypeEnum.Pierce_660_nm);
                    break;
                case "btnDiagnostics":
                    GlobalProvider.MeasureUserCtrl = new UserDiagnostics();
                    break;
                case "btnCheckLight":
                    GlobalProvider.MeasureUserCtrl = new UserCheckLight();
                    break;
                case "btnUtility":
                    GlobalProvider.MeasureUserCtrl = new UserUtilityCtrl();
                    break;
            }
            if (btn.Name == "btnDiagnostics" || btn.Name == "btnCheckLight")
            {
                btnPrint.Visibility = Visibility.Visible;
                btnExport.Visibility = Visibility.Visible;
                btnBlank.Visibility = Visibility.Hidden;
                btnMeasure.Visibility = Visibility.Hidden;
            }
            else
            {
                btnPrint.Visibility = Visibility.Visible;
                btnExport.Visibility = Visibility.Visible;
                btnBlank.Visibility = Visibility.Visible;
                btnMeasure.Visibility = Visibility.Visible;
                btnMeasure.IsEnabled = false;
            }
            if(btn.Name=="btnUtility")
            {
                btnPrint.Visibility = Visibility.Hidden;
                btnExport.Visibility = Visibility.Hidden;
                btnBlank.Visibility = Visibility.Hidden;
                btnMeasure.Visibility = Visibility.Hidden;
            }
            borderMeasure.Child = (UIElement)GlobalProvider.MeasureUserCtrl;
            tabMain.SelectedIndex = 1;
        }

        #endregion

        #region 测试
        //空值
        private void btnBlank_Click(object sender, RoutedEventArgs e)
        {
            btnMeasure.IsEnabled = true;
            GlobalProvider.MeasureUserCtrl.Blank();
        }
        //测量样本
        private void btnMeasure_Click(object sender, RoutedEventArgs e)
        {
            GlobalProvider.MeasureUserCtrl.Measure();
            
        }
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            GlobalProvider.MeasureUserCtrl.ExportTxt();
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DyeListBLL.ReadInitDye();
            try
            {
                if (OmniProvider.OpenSpectrometers() > 0)
                {
                    OmniProvider.OpenSerialPort();
                    OmniProvider.SendCommand("ZZ",false);
                    OmniProvider.GetWaveLengthsList();//获得波长
                    OmniProvider.GetParameters();
                }
                else
                {
                    MessageBox.Show("Connect fail.未找到光谱仪");
                    Functions.CommnucationLog("Connect fail.未找到光谱仪");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Connect fail." + ex.Message);
                Functions.CommnucationLog("Connect fail."+ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OmniProvider.CloseAllSpectrometers();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            dialog.PrintVisual(borderMeasure,"mesure");
        }

    }
}
