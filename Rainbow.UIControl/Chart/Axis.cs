using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Rainbow.UIControl.Chart
{
    /// <summary>
    /// 坐标轴的方法和属性
    /// </summary>
    public class Axis : Control
    {
        #region 变量
        string mTitle;//坐标轴标题
        double mValueScale;//值与坐标关系
        double mMaxValue=1000;//最大值
        double mMinValue=0;//最小值
        int mStepCount;//组数
        double mStepValue;//组距

        double mLogBase = 10;//对数底数

        ChartArea mChartArea;//所属图表
        AxisTypeName mAxisTypeName;//坐标轴类型名
        Canvas mAxisCanvas;
        double mChartHeight;
        double mChartWidth;

        #endregion

        #region 属性
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return mTitle;
            }
            set
            {
                mTitle = value;
            }
        }
        /// <summary>
        /// 获取值与坐标的比例参数
        /// </summary>
        public double ValueScale
        {
            get
            {
                return mValueScale;
            }
        }
        /// <summary>
        /// 获取或设置指示相应轴是否为对数轴的标志。对数图表中不允许出现零或负的数据值。
        /// </summary>
        [DefaultValue(false)]
        public bool IsLogarithmic
        {
            get;
            set;
        }
        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue
        {
            get
            {
                return mMaxValue;
            }
            set
            {
                mMaxValue = value;
            }
        }
        
        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue
        {
            get
            {
                return mMinValue;
            }
            set
            {
                mMinValue = value;
            }
        }
        /// <summary>
        ///组数
        /// </summary>
        public int StepCount
        {
            get
            {
                return mStepCount;
            }
            set
            {
                mStepCount = value;
            }
        }
        /// <summary>
        /// 对数底数(默认为10）
        /// </summary>
        public double LogBase
        {
            get
            {
                return mLogBase;
            }
            set
            {
                mLogBase = value;
            }
        }

        #endregion

        public Axis()
        {
            DependencyObject parent = LogicalTreeHelper.GetParent(this); //VisualTreeHelper.GetParent(this);

            while (parent != null)
            {
                if (parent is ChartArea)
                {
                    mChartArea = (ChartArea)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="chartArea">所属图表</param>
        /// <param name="axisTypeName"></param>
        public Axis(ChartArea chartArea, AxisTypeName axisTypeName)
        {
            mChartArea = chartArea;
            mAxisTypeName = axisTypeName;

        }

        #region 直角坐标

        public void CreateAxis()
        {
            CreateAxis(MaxValue, MinValue, StepCount);
        }
        /// <summary>
        /// 建立固定组数的坐标轴
        /// </summary>
        /// <param name="maxValue">最大值</param>
        /// <param name="stepCount">组数</param>
        public void CreateAxis(double maxValue, int stepCount)
        {
            CreateAxis(maxValue, 0, stepCount);
        }
        /// <summary>
        /// 建立固定组数的坐标轴
        /// </summary>
        /// <param name="maxValue">最大值</param>
        /// <param name="minValue">最小值</param>
        /// <param name="stepCount">组数</param>
        public void CreateAxis(double maxValue, double minValue, int stepCount)
        {
            mChartHeight = mChartArea.DataArea.ActualHeight;//图表的高和宽
            mChartWidth = mChartArea.DataArea.ActualWidth;
            mMaxValue = maxValue;
            mMinValue = minValue;
            mStepCount = stepCount;
            mStepValue = (mMaxValue - mMinValue) / mStepCount; //计算组距

            //绘制轴
            switch (mAxisTypeName)
            {
                case AxisTypeName.X:
                    CreateAxisX();
                    break;
                case AxisTypeName.Y:
                    CreateAxisY();
                    break;
            }
        }
        //创建X轴
        private void CreateAxisX()
        {
            mValueScale = (mChartWidth - 20) / (mMaxValue - mMinValue);//计算X值与坐标关系，留出20边界
            mAxisCanvas = mChartArea.cvsAxisX;
            mAxisCanvas.Children.Clear();
            Rectangle rectangleX = new Rectangle()
            {
                Height = 1,
                Width = mChartWidth,
                Fill = Brushes.Black
            };
            Canvas.SetTop(rectangleX, mChartHeight);
            Canvas.SetLeft(rectangleX, 0);
            mChartArea.DataArea.Children.Add(rectangleX);
            for (int i = 0; i <= mStepCount; i++)
            {
                if (i != 0)
                {
                    //绘制X轴刻度
                    Rectangle rectangle = new Rectangle();
                    rectangle.Height = 5;
                    rectangle.Width = 1;
                    rectangle.Fill = Brushes.Black;
                    Canvas.SetLeft(rectangle, mStepValue * mValueScale * i);
                    Canvas.SetBottom(rectangle, 0);
                    mChartArea.DataArea.Children.Add(rectangle);
                }
                //X轴标签
                TextBlock lbl = new TextBlock();
                lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lbl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lbl.Text = (mMinValue + mStepValue * i).ToString();
                double width = lbl.Text.Length * lbl.FontSize;

                mAxisCanvas.Children.Add(lbl);
                this.UpdateLayout();
                Canvas.SetLeft(lbl, mStepValue * mValueScale * i - (lbl.RenderSize.Width / 2));
                Canvas.SetTop(lbl, 5);
            }
        }
        //创建Y轴
        private void CreateAxisY()
        {
            mValueScale = (mChartHeight - 20) / (mMaxValue - mMinValue);//计算Y值与坐标关系，留出20边界
            mAxisCanvas = mChartArea.cvsAxisY;
            mAxisCanvas.Children.Clear();
            //绘制Y轴
            Rectangle rectangleY = new Rectangle()
            {
                Height = mChartHeight,
                Width = 1,
                Fill = Brushes.Black
            };
            Canvas.SetTop(rectangleY, 0);
            Canvas.SetLeft(rectangleY, 0);
            mChartArea.DataArea.Children.Add(rectangleY);
            for (int i = 0; i <= mStepCount; i++)
            {
                //绘制Y轴刻度
                if (i != 0)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Height = 1;
                    rectangle.Width = 5;
                    rectangle.Fill = Brushes.Black;
                    Canvas.SetLeft(rectangle, 0);
                    Canvas.SetBottom(rectangle, mStepValue * mValueScale * i);
                    mChartArea.DataArea.Children.Add(rectangle);

                    //Y轴标签
                    TextBlock lbl = new TextBlock();
                    lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    lbl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    lbl.Text = (mMinValue + mStepValue * i).ToString();
                    Canvas.SetRight(lbl, 5);
                    Canvas.SetBottom(lbl, mStepValue * mValueScale * i - 7);
                    mAxisCanvas.Children.Add(lbl);
                }
            }
        }
        #endregion

    }
}
