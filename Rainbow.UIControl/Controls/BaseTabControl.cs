using System;
using System.Windows;
using System.Windows.Controls;
namespace ClipperBaseUI.Controls
{
	public class BaseTabControl : TabControl
	{
        static BaseTabControl()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseTabControl), new FrameworkPropertyMetadata(typeof(BaseTabControl)));
		}
	}
}
