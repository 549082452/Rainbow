using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace ClipperBaseUI.Controls
{
	public class BaseTabItem : TabItem
	{
		public static readonly DependencyProperty MyMoverBrushProperty;
		public static readonly DependencyProperty MyEnterBrushProperty;
		public Brush MyMoverBrush
		{
			get
			{
				return base.GetValue(BaseTabItem.MyMoverBrushProperty) as Brush;
			}
			set
			{
				base.SetValue(BaseTabItem.MyMoverBrushProperty, value);
			}
		}
		public Brush MyEnterBrush
		{
			get
			{
				return base.GetValue(BaseTabItem.MyEnterBrushProperty) as Brush;
			}
			set
			{
				base.SetValue(BaseTabItem.MyEnterBrushProperty, value);
			}
		}
		static BaseTabItem()
		{
			BaseTabItem.MyMoverBrushProperty = DependencyProperty.Register("MyMoverBrush", typeof(Brush), typeof(BaseTabItem), new PropertyMetadata(null));
			BaseTabItem.MyEnterBrushProperty = DependencyProperty.Register("MyEnterBrush", typeof(Brush), typeof(BaseTabItem), new PropertyMetadata(null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseTabItem), new FrameworkPropertyMetadata(typeof(BaseTabItem)));
		}
        public BaseTabItem()
		{
            //base.Header = "测试按钮";
            //base.Background = Brushes.Blue;
		}
	}
}
