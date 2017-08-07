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

namespace Rainbow.UIControl.Controls
{
    /// <summary>
    /// NumTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class NumTextBox : TextBox
    {
        public NumTextBox()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获得控件的数字,如果是空返回0
        /// </summary>
        public int IntNumber
        {
            get
            {
                int i = 0;
                int.TryParse(Text, out i);
                return i;
            }
            set
            {
                Text = value.ToString(); ;
            }
        }
        /// <summary>
        /// 获得控件的数字
        /// </summary>
        public double DoubleNumber
        {
            get
            {
                double i = 0;
                double.TryParse(Text, out i);
                return i;
            }
            set
            {
                Text = value.ToString();
            }
        }
        ///// <summary>
        ///// 获得/设置文本
        ///// </summary>
        //public string Text
        //{
        //    get
        //    {
        //        return txt1.Text;
        //    }
        //    set
        //    {
        //        txt1.Text = value;
        //    }
        //}

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;

            //屏蔽非法按键
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal)
            {
                if (txt.Text.Contains(".") && e.Key == Key.Decimal)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) ||
            e.Key == Key.OemPeriod) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (txt.Text.Contains(".") && e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
            //屏蔽中文输入和非法字符粘贴输入
            //TextBox textBox = sender as TextBox;
            //TextChange[] change = new TextChange[e.Changes.Count];
            //e.Changes.CopyTo(change, 0);

            //int offset = change[0].Offset;
            //if (change[0].AddedLength > 0)
            //{
            //    double num = 0;
            //    if (!Double.TryParse(textBox.Text, out num))
            //    {
            //        textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
            //        textBox.Select(offset, 0);
            //    }
            //}
        //}
        
    }
}
