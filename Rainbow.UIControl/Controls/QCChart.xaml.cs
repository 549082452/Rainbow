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

namespace ClipperBaseUI.Controls
{
    /// <summary>
    /// QCChart.xaml 的交互逻辑
    /// </summary>
    public partial class QCChart : UserControl
    {
        public QCChart()
        {
            InitializeComponent();
        }

        #region 变量
        //图标区域的高度和宽度
        double mChartHeight;
        double mChartWidth;
        double mYValueScale;//Y轴值与坐标关系
        double mYMaxValue;
        double mXValueScale;//X轴值与坐标关系

        double mVariance;//方差
        double mSD;//标准差
        double mAverage;//均值
        ObservableCollection<Point> mQCData = null;//质控数据
        double mCV;//变异系数
        #endregion

        #region 属性
        /// <summary>
        /// 方差
        /// </summary>
        public double Variance
        {
            get
            {
                return mVariance;
            }
        }
        /// <summary>
        /// 标准差
        /// </summary>
        public double SD
        {
            get
            {
                return mSD;
            }
        }
        /// <summary>
        /// 平均值
        /// </summary>
        public double Average
        {
            get
            {
                return mAverage;
            }
        }
        /// <summary>
        /// 变异系数
        /// </summary>
        public double CV
        {
            get
            {
                return mCV;
            }
        }

        #endregion
        /// <summary>
        /// 显示质控曲线
        /// </summary>
        /// <param name="sd">标准差</param>
        /// <param name="average">平均值</param>
        /// <param name="QCData">质控数据</param>
        public void ShowData(ObservableCollection<Point> QCData)
        {
            chartArea.Children.Clear();
            mChartHeight = chartArea.ActualHeight;
            mChartWidth = chartArea.ActualWidth;
            CalcSD(QCData);
            CreateAxisY();
            CreateAxisX(QCData.Count);
            mQCData = QCData;
            bool isFirstPoint = true;
            Point lastPointPosition = new Point();//上一个点的位置
            foreach (Point point1 in QCData)
            {
                //画点
                Ellipse p1 = new Ellipse();
                p1.Height = 4;
                p1.Width = 4;
                p1.MaxHeight = 4;
                p1.MaxWidth = 4;
                p1.Fill = Brushes.Black;
                Point pointPosition = new Point(point1.X * mXValueScale - 2, mChartHeight / 2 - ((point1.Y-mAverage) * mYValueScale) - 2);

                Canvas.SetLeft(p1, pointPosition.X);
                Canvas.SetTop(p1, pointPosition.Y);//位置坐标对齐圆心所以要减去2
                chartArea.Children.Add(p1);
                //连线
                if (!isFirstPoint)
                {
                    Line line = new Line()
                    {
                        StrokeThickness = 1,

                        Stroke = Brushes.Black
                    };
                    line.X1 = lastPointPosition.X + 2;//连接圆心
                    line.Y1 = lastPointPosition.Y + 2;
                    line.X2 = pointPosition.X + 2;
                    line.Y2 = pointPosition.Y + 2;
                    chartArea.Children.Add(line);
                }
                else
                {
                    isFirstPoint = false;
                }
                lastPointPosition = pointPosition;
            }
        }

        /// <summary>
        /// 建立X轴
        /// </summary>
        private void CreateAxisX(int count)
        {
            mXValueScale = mChartWidth / (count + 1);
        }
        /// <summary>
        /// 建立Y轴
        /// </summary>
        private void CreateAxisY()
        {
            lblYAxis1.Content = (mAverage + 3 * mSD).ToString("f2");
            lblYAxis2.Content = (mAverage + 2 * mSD).ToString("f2");
            lblYAxis3.Content = (mAverage + mSD).ToString("f2");
            lblYAxis4.Content = mAverage.ToString("f2");
            lblYAxis5.Content = (mAverage - mSD).ToString("f2");
            lblYAxis6.Content = (mAverage - 2 * mSD).ToString("f2");
            lblYAxis7.Content = (mAverage - 3 * mSD).ToString("f2");
            mYMaxValue = mAverage + 3 * mSD;
            mYValueScale = mChartHeight / mYMaxValue / 2;//计算Y值与坐标关系
        }

        private void chartArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (mQCData != null)
            {
                chartArea.Children.Clear();
                ShowData(mQCData);
            }
        }
        /// <summary>
        /// 计算点中Y值的标准差
        /// </summary>
        /// <param name="numCollection">数据点</param>
        private void CalcSD(ObservableCollection<Point> numCollection)
        {
            mAverage= numCollection.Average(n => n.Y);
            foreach (Point num in numCollection)
            {
                mVariance += Math.Pow((num.Y - mAverage), 2);
            }
            mSD = Math.Pow(mVariance, 0.5);
            mCV = mSD / mAverage * 100;
        }
        
    }
}
