using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.ComponentModel;

namespace Rainbow.UIControl.Chart
{
    /// <summary>
    /// 存储数据点和序列特性。
    /// </summary>
    public class Series : Control
    {
        #region 变量

        ObservableCollection<Point> mDataPoint;//数据点
        ObservableCollection<Point> mLinePoint;//画线的点
        double mXValueScale;
        double mYValueScale;
        double mXLogBase;
        double mYLogBase;
        bool isShowMark = true;
        bool isShowLine = true;

        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置所属的ChartArea
        /// </summary>
        public ChartArea ChartArea
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置ChartArea的名字
        /// </summary>
        public string ChartAreaName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示点
        /// </summary>
        [DefaultValue(true)]
        public bool IsShowMark
        {
            get
            {
                return isShowMark;
            }
            set
            {
                isShowMark = value;
            }
        }
        /// <summary>
        /// 显示线
        /// </summary>
        public bool IsShowLine
        {
            get
            {
                return isShowLine;
            }
            set
            {
                isShowLine = value;
            }
        }
        /// <summary>
        /// 曲线的点数据
        /// </summary>
        public ObservableCollection<Point> DataPoint
        {
            get
            {
                if (mDataPoint == null)
                {
                    mDataPoint = new ObservableCollection<Point>();
                }
                return mDataPoint;
            }
            set
            {
                mDataPoint = value;
            }
        }
        /// <summary>
        /// 画线的数据
        /// </summary>
        public ObservableCollection<Point> LinePoint
        {
            get
            {
                if (mLinePoint == null)
                    mLinePoint = new ObservableCollection<Point>();
                return mLinePoint;
            }
            set
            {
                mLinePoint = value;
            }
        }
        #endregion

        public Series()
        {
        }
        /// <summary>
        /// 序列名称
        /// </summary>
        /// <param name="name"></param>
        public Series(string name, ChartArea chartArea)
        {
            Name = name;
            ChartArea = chartArea;
        }
        /// <summary>
        /// 绘制曲线
        /// </summary>
        public void ShowCurve()
        {
            if (mDataPoint != null)
            {
                //if (IsShowMark)//显示点
                //{
                //    ShowData(mDataPoint);
                //}
                if (IsShowLine)//显示线
                {
                    ShowLine(mLinePoint);
                }
            }

        }

        public void ShowLine(double[] yValue)
        {
            ObservableCollection<Point> pointList = new ObservableCollection<Point>();
            if (yValue != null)
            {
                for (int i = 29; i < yValue.Length; i++)//从200波长开始
                {
                    Point point = new Point(i, yValue[i]);
                    pointList.Add(point);
                }
            }
            ShowLine(pointList);
        }

        public void ShowLine(double[] xValue,double[] yValue)
        {
            ObservableCollection<Point> pointList = new ObservableCollection<Point>();
            if (yValue != null)
            {
                for (int i = 29; i < yValue.Length; i++)
                {
                    Point point = new Point(xValue[i], yValue[i]);
                    pointList.Add(point);
                }
            }
            ShowLine(pointList);
        }
        /// <summary>
        /// 绘制曲线,使用数组
        /// </summary>
        /// <param name="value">数组值</param>
        public void ShowData(Array yValue)
        {
            if (yValue != null)
            {
                int i = 1;
                foreach (double value in yValue)
                {
                    Point point = new Point(i, value);
                    ShowData(point);
                    i++;
                }
            }
        }

        /// <summary>
        /// 绘制一个数据点
        /// </summary>
        /// <param name="point"></param>
        public void ShowData(Point point)
        {
            Ellipse p1 = new Ellipse();
            p1.Height = 4;
            p1.Width = 4;
            p1.MaxHeight = 4;
            p1.MaxWidth = 4;
            p1.Fill = Brushes.Black;
            //位置坐标对齐圆心所以要减去2
            Point pointPosition = DataToPrintPoint(point, 2, 2);
            Canvas.SetLeft(p1, pointPosition.X);
            Canvas.SetBottom(p1, pointPosition.Y);
            ChartArea.DataArea.Children.Add(p1);
        }


        /// <summary>
        /// 绘制线
        /// </summary>
        /// <param name="point">数据</param>
        public virtual void ShowLine(ObservableCollection<Point> points)
        {        
            bool isFirstPoint = true;
            Point lastPointPosition = new Point();//上一个点的位置
            if (points != null)
            {
                foreach (Point point in points)
                {
                    Ellipse p1 = new Ellipse();
                    p1.Height = 1;
                    p1.Width = 1;
                    p1.MaxHeight = 1;
                    p1.MaxWidth = 1;
                    p1.Fill = Brushes.Black;
                    //位置坐标对齐圆心所以要减去0.5
                    Point pointPosition = DataToPrintPoint(point, 0, 0);
                    //Canvas.SetLeft(p1, pointPosition.X);
                    //Canvas.SetBottom(p1, pointPosition.Y);
                    //ChartArea.DataArea.Children.Add(p1);

                    if (!isFirstPoint)
                    {
                        #region create method3

                        Path pp3 = new Path();

                        //pp3.Data

                        pp3.Stroke = new SolidColorBrush(Colors.Black);

                        pp3.StrokeThickness = 0.8;

                        GeometryConverter gc = new GeometryConverter();
                        pp3.Data = (Geometry)gc.ConvertFromString("M " + lastPointPosition.X + "," + lastPointPosition.Y + " " + pointPosition.X + "," + pointPosition.Y);
                        ChartArea.DataArea.Children.Add(pp3);
                    }
                    else
                    {
                        isFirstPoint = false;
                    }
                    #endregion

                    lastPointPosition = pointPosition;
                }
            }
        }

   

        /// <summary>
        /// 计算点的绘图坐标
        /// </summary>
        /// <param name="point">点数据</param>
        /// <param name="correctX">X位移</param>
        /// <param name="correctY">Y位移</param>
        /// <returns>位置</returns>
        public Point DataToPrintPoint(Point point, double correctX, double correctY)
        {
            Point pointPosition = new Point();
            //设置坐标系的比例参数
            mXValueScale = ChartArea.AxisX.ValueScale;
            mYValueScale = ChartArea.AxisY.ValueScale;

            switch (ChartArea.CoordinateType)//不同坐标系中显示方法
            {
                case CoordinateInfoEnum.XY:
                    pointPosition.X = (point.X - ChartArea.AxisX.MinValue) * mXValueScale - correctX;
                    pointPosition.Y = ChartArea.DataArea.ActualHeight - (point.Y - ChartArea.AxisY.MinValue) * mYValueScale - correctY;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return pointPosition;
        }
        /// <summary>
        /// 计算坐标点的值
        /// </summary>
        /// <param name="point">点数据</param>
        /// <param name="correctX">X位移</param>
        /// <param name="correctY">Y位移</param>
        /// <returns>位置</returns>
        public Point PrintPointToData(Point pointPosition, double correctX, double correctY)
        {
            Point point = new Point();
            //设置坐标系的比例参数
            mXValueScale = ChartArea.AxisX.ValueScale;
            mYValueScale = ChartArea.AxisY.ValueScale;
            switch (ChartArea.CoordinateType)//不同坐标系中显示方法
            {
                case CoordinateInfoEnum.XY:
                    point.X=  (pointPosition.X + correctX) / mXValueScale +ChartArea.AxisX.MinValue ;
                    point.Y = (ChartArea.DataArea.ActualHeight - pointPosition.Y - correctY) / mYValueScale + ChartArea.AxisY.MinValue;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return point;
        }

        /// <summary>
        /// 平滑采样数据
        /// </summary>
        /// <param name="pDataBuffer"></param>
        /// <param name="bufferSize"></param>
        /// <param name="shiftLength"></param>
        void ShiftMeanFilter(double[] pDataBuffer, long bufferSize, short shiftLength)
        {
            //滑动平均滤波
            //确定采样队列长度
            if (shiftLength < 4)
            {
                shiftLength = 4;
            }
            if (shiftLength > 100)
            {
                shiftLength = 100;
            }

            double dbSum = 0;
            double lastModify;

            //计算第一采样队列的和并记住第一个位置的原始值
            for (short k = 0; k < shiftLength; k++)
            {
                dbSum += pDataBuffer[k];
            }
            lastModify = pDataBuffer[0];
            pDataBuffer[0] = (double)(dbSum / shiftLength);

            //开始向后滑动
            long i;
            for (i = 1; i < bufferSize - shiftLength; i++)
            {
                dbSum += pDataBuffer[i + shiftLength] - lastModify;
                lastModify = pDataBuffer[i];
                pDataBuffer[i] = (double)(dbSum / shiftLength);
            }

            //最后一个采样队列所有的值都用i-1次获得的值填充
            double lastMeanValue = pDataBuffer[i - 1];
            while (i < bufferSize)
            {
                pDataBuffer[i] = lastMeanValue;
                i++;
            }

        }

    }
}
