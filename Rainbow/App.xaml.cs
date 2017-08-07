using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Globalization;
using Rainbow.Utility.Functions;
using System.Windows.Threading;

namespace Rainbow
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Application_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        }
        //未捕获的非窗体异常
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.ExceptionObject as Exception;
                Functions.LOG("CurrentDomain," + ex.Message + "," + ex.Source + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
                Functions.LOG("ExitExc");
            }
        }
        //未捕获的窗体异常，或者调用dispatcher传递的异常
        void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {

                Exception ex = e.Exception as Exception;
                Functions.LOG("Dispatcher," + ex.Message + "," + ex.Source + Environment.NewLine + ex.StackTrace);
                var comException = e.Exception as System.Runtime.InteropServices.COMException;
                if (comException != null && comException.ErrorCode == -2147221040)///OpenClipboard HRESULT:0x800401D0 (CLIPBRD_E_CANT_OPEN)) 剪贴板异常处理
                {
                    e.Handled = true;
                    return;
                }
                Functions.LOG(ex.Message);
                e.Handled = true;
            }
            catch
            {
                //Show("不可恢复的WPF窗体线程异常，应用程序将退出！");
                Functions.LOG("ExitExc");
            }
        }


        #region 语言Method
        public static string Culture { get; set; }

        private void GetCulture()
        {
            Culture = string.Empty;
            try
            {
                //Culture = Rainbow.Properties.Settings.Default.Culture.Trim();
            }
            catch (Exception)
            {
            }
            Culture = string.IsNullOrEmpty(Culture) ? "en-US" : Culture;

            //update culture
            UpdateCulture();
        }
        private void SetCulture()
        {
            try
            {
                //Rainbow.Properties.Settings.Default.Culture = Culture;
                //Rainbow.Properties.Settings.Default.Save();
            }
            catch (Exception)
            {
            }
        }
        public static void UpdateCulture()
        {
            string requestedCulture = string.Format(@"Resources\StringResource.{0}.xaml", Culture);
            ResourceDictionary resourceDictionary = null;
            //if (File.Exists(requestedCulture))
            //{
            try
            {
                resourceDictionary = Application.LoadComponent(new Uri(requestedCulture, UriKind.Relative)) as ResourceDictionary;
            }
            catch
            {

            }
            if (resourceDictionary == null)
            {
                requestedCulture = @"Resources\StringResource.xaml";
                resourceDictionary = Application.LoadComponent(new Uri(requestedCulture, UriKind.Relative)) as ResourceDictionary;
            }
            if (resourceDictionary != null)
            {
                //Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Culture);

        }
        #endregion

    }
}
