using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace ClipperBaseUI.Controls
{
	public class BaseButton : Button
	{
		public static readonly DependencyProperty MyMoverBrushProperty;
		public static readonly DependencyProperty MyEnterBrushProperty;
		public Brush MyMoverBrush
		{
			get
			{
				return base.GetValue(BaseButton.MyMoverBrushProperty) as Brush;
			}
			set
			{
				base.SetValue(BaseButton.MyMoverBrushProperty, value);
			}
		}
		public Brush MyEnterBrush
		{
			get
			{
				return base.GetValue(BaseButton.MyEnterBrushProperty) as Brush;
			}
			set
			{
				base.SetValue(BaseButton.MyEnterBrushProperty, value);
			}
		}
		static BaseButton()
		{
			BaseButton.MyMoverBrushProperty = DependencyProperty.Register("MyMoverBrush", typeof(Brush), typeof(BaseButton), new PropertyMetadata(null));
			BaseButton.MyEnterBrushProperty = DependencyProperty.Register("MyEnterBrush", typeof(Brush), typeof(BaseButton), new PropertyMetadata(null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseButton), new FrameworkPropertyMetadata(typeof(BaseButton)));
		}
        public BaseButton()
		{
            //base.Content = "";
            //base.Background = Brushes.Orange;
		}
	}
}
