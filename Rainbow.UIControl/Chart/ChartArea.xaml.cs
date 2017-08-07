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
using System.ComponentModel;

namespace Rainbow.UIControl.Chart
{
    /// <summary>
    /// ChartArea.xaml 的交互逻辑
    /// </summary>
    public partial class ChartArea : UserControl
    {
        public ChartArea()
        {
            InitializeComponent();
        }
        #region 变量

        Axis mAxisX = null;
        Axis mAxisY = null;
        CoordinateInfoEnum mCoordinateType = 0;//默认是X-Y
        List<Series> mSeriesCollection = null;

        #endregion
        public delegate void ChangeSetWave(Point pointValue);
        public event ChangeSetWave ChangeSetWaveEvent;
        #region 属性
        /// <summary>
        /// 获得或设置X轴
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public Axis AxisX
        {
            get
            {
                if (mAxisX == null)
                {
                    mAxisX = new Axis(this, AxisTypeName.X);
                }
                return mAxisX;
            }
            set
            {
                mAxisX = value;
            }
        }

        /// <summary>
        /// 获得或设置Y轴
        /// </summary>
        public Axis AxisY
        {
            get
            {
                if (mAxisY == null)
                {
                    mAxisY = new Axis(this, AxisTypeName.Y);
                }
                return mAxisY;
            }
            set
            {
                mAxisY = value;
            }
        }
        /// <summary>
        /// 坐标系类型(默认是X-Y)
        /// </summary>
        public CoordinateInfoEnum CoordinateType
        {
            get
            {
                return mCoordinateType;
            }
            set
            {
                mCoordinateType = value;
            }
        }
        /// <summary>
        /// 数据序列的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public List<Series> SeriesCollection
        {
            get
            {
                if (mSeriesCollection == null)
                {
                    mSeriesCollection = new List<Series>();
                }
                return mSeriesCollection;
            }
            set
            {
                mSeriesCollection = value;
            }
        }
        public Canvas CvsAxisX
        {
            get
            {
                return cvsAxisX;
            }
            set
            {
                value = cvsAxisX;
            }
        }
        public double SetWaveLength
        {
            get;
            set;

        }
        #endregion
        #region 方法
        /// <summary>
        /// 建立坐标系
        /// </summary>
        public void CreateCoordinate()
        {
            switch (CoordinateType)//不同类型的坐标系
            {
                case CoordinateInfoEnum.XY:
                    AxisX.CreateAxis();
                    AxisY.CreateAxis();
                    break;
              
                default:
                    throw new NotImplementedException();
            }

        }
        public void CreateCoordinate(int xMin, int xMax, int yMin, int yMax)
        {
            AxisX.CreateAxis(xMax,10);
            AxisY.CreateAxis(yMax,10);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        public void ShowData(double[] yValue,bool isRemoveOtherSeries)
        {
            AxisX = new Axis(this, AxisTypeName.X);
            AxisY = new Axis(this, AxisTypeName.Y);
            AxisX.MaxValue = 2048;
            AxisX.MinValue = 0;
            AxisX.StepCount = 10;
            AxisY.MaxValue = 2000;
            AxisY.MinValue = 0;
            AxisY.StepCount = 10;
            
            Series series = new Series("series1", this);
            if (isRemoveOtherSeries)
            {
                DataArea.Children.Clear();
                SeriesCollection.Clear();
            }
            SeriesCollection.Add(series);
            CreateCoordinate();
            SeriesCollection[0].ShowLine(yValue);
        }

        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="standardInfo"></param>
        public void ShowData(double[] xValue, double[] yValue, double xMin, double yMin, double xMax, double yMax, bool isRemoveOtherSeries)
        {
            AxisX = new Axis(this, AxisTypeName.X);
            AxisY = new Axis(this, AxisTypeName.Y);
            AxisX.MaxValue = xMax;
            AxisX.MinValue = xMin;
            AxisX.StepCount = 10;
            AxisY.MaxValue = yMax;
            AxisY.MinValue = yMin;
            AxisY.StepCount = 10;
           
            Series series = new Series("series1", this);
            if (isRemoveOtherSeries)
            {
                DataArea.Children.Clear();
                SeriesCollection.Clear();
            } 
            
            CreateCoordinate();
            SeriesCollection.Add(series);
            SeriesCollection[0].ShowLine(xValue, yValue);
        }

        /// <summary>
        /// 获得不小于自身，除最高为外全为0的数，如111得到200，100得到100
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private double Get10Multiple(double num)
        {
            string _max = num.ToString();
            int _behindNum = int.Parse(_max.Substring(1, _max.Length - 1));
            if (_behindNum > 0)
            {
                int _pow = _max.Length - 1;
                int _firstNum = int.Parse(_max.Substring(0, 1)) + 1;
                num = _firstNum * Math.Pow(10, _pow);
            }
            return num;
        }
        public void Clear()
        {
            cvsAxisX.Children.Clear();
            cvsAxisY.Children.Clear();
            DataArea.Children.Clear();
        }

        public void SetWave(double waveLength)
        {

            if (SeriesCollection.Count > 0)
            {
                Point point= SeriesCollection[0].DataToPrintPoint(new Point(waveLength, 0), 0, 0);
                if (lineY == null)
                {
                    lineY = new Line();
                DataArea.Children.Add(lineY);
                }
                lineY.X1 = point.X;
                lineY.X2 = point.X;
                lineY.Y1 = 0;
                lineY.Y2 = DataArea.ActualHeight;
                lineY.Stroke = Brushes.Black;
                lineY.StrokeThickness = 1;
            }
        }
        #endregion
    
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (cvsAxisY.Children.Count > 0)
            //{
            //    cvsAxisX.Children.Clear();
            //    cvsAxisY.Children.Clear();
            //    DataArea.Children.Clear();
            //    //ShowData();
            //}
        }

        #region 事件
        Line lineX = null;
        Line lineY = null;
        private void DataArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (lineX == null)
            {
                lineX = new Line();
                lineY = new Line();
                DataArea.Children.Add(lineX);
                DataArea.Children.Add(lineY);
            }

            Point point = e.GetPosition(DataArea);
            Console.WriteLine(point);

            //横线
            lineX.X1 = 0;
            lineX.Y1 = point.Y;
            lineX.X2 = DataArea.ActualWidth;
            lineX.Y2 = point.Y;
            lineX.Stroke = Brushes.Black;
            lineX.StrokeThickness = 1;

            //竖线
            lineY.X1 = point.X;
            lineY.X2 = point.X;
            lineY.Y1 = 0;
            lineY.Y2 = DataArea.ActualHeight;
            lineY.Stroke = Brushes.Black;
            lineY.StrokeThickness = 1;

        }
         private void DataArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lineY == null)
            {
                lineY = new Line();
                DataArea.Children.Add(lineY);
            }

            Point point = e.GetPosition(DataArea);
            Console.WriteLine(point);

            //竖线
            lineY.X1 = point.X;
            lineY.X2 = point.X;
            lineY.Y1 = 0;
            lineY.Y2 = DataArea.ActualHeight;
            lineY.Stroke = Brushes.Black;
            lineY.StrokeThickness = 1;
            if (ChangeSetWaveEvent != null)
            {
                if (SeriesCollection.Count > 0)
                {
                    Point pointValue = SeriesCollection[0].PrintPointToData(point, 0, 0);
                    ChangeSetWaveEvent.Invoke(pointValue);
                }
            }
        }
        #endregion 

      
    }
}
