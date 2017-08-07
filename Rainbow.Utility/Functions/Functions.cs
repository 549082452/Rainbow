using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rainbow.Utility.Functions
{
    /// <summary>
    /// 公用方法
    /// </summary>
    public class Functions
    {
        #region 程序集特性访问器

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        #region 写入日志方法
        private static string mLogDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Log";// Environment.CurrentDirectory + "\\Log";
        private static Mutex mMutex = new Mutex();
        static bool m_isAccessDenied = false;
        #region // LOG(string log)
        /// <summary>
        /// 记录日志,显示分秒毫秒
        /// </summary>
        /// <param name="log">内容</param>
        public static void LOG(string log)
        {
            if (!Directory.Exists(mLogDirectory))
            {
                Directory.CreateDirectory(mLogDirectory);
            }

            log = string.Format("{0},{1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString())
                + "  [" + GlobalProvider.CurrentClass + "] "
                 + log;
            try
            {
                FileStream _stream = File.Open(mLogDirectory + "\\log.txt", FileMode.Append);
                StreamWriter _writer = new StreamWriter(_stream);
#if DEBUG
                Console.WriteLine(log);
#endif
                //MessageBox.Show(log);
                _writer.WriteLine(log);
                _writer.Close();
                _stream.Close();
            }
            catch (Exception Ex)
            {
                Debug.Print(Ex.Message);
                m_isAccessDenied = true;
                return;
            }
        }
        #endregion

        #region // LOG(string className, string log)
        /// <summary>
        /// 记录日志,显示分秒毫秒
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="log">日志内容</param>
        public static void LOG(string className, string log)
        {
            if (m_isAccessDenied) return;
            string _commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (!Directory.Exists(mLogDirectory))
            {
                Directory.CreateDirectory(mLogDirectory);
            }
            log = string.Format("{0},{1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString())
                + string.Format("{0:D2}:{1:D2}.{2:D3}", DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond)
                + "  [" + className + "] "
                 + log;
            try
            {
                FileStream _stream = File.Open(mLogDirectory + "\\log.txt", FileMode.Append);
                StreamWriter _writer = new StreamWriter(_stream);
                //if (ServiceProvider.IsSimulate)
                //{
                Console.WriteLine(log);
                //}
                _writer.WriteLine(log);
                _writer.Close();
                _stream.Close();
            }
            catch (Exception Ex)
            {
                Debug.Print(Ex.Message);
                m_isAccessDenied = true;
                return;
            }
        }
        #endregion

        #region // LOG(DateTime timestamp, string log)
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="log">日志内容</param>
        public static void LOG(DateTime timestamp, string log)
        {

            string _logPath = mLogDirectory + "\\log.txt";
            mMutex.WaitOne();
            LogStart_Backup_End(timestamp, log, _logPath);
            mMutex.ReleaseMutex();
        }
        /// <summary>
        /// 记录上下位机通讯
        /// </summary>
        /// <param name="log"></param>
        public static void CommnucationLog(string log)
        {
            string _logPath = mLogDirectory + "\\CmdLog.txt";
            mMutex.WaitOne();
            LogStart_Backup_End(null,DateTime.Now+":"+ log, _logPath);
            mMutex.ReleaseMutex();
        }

        private static void LogStart_Backup_End(DateTime? timestamp, string log, string _logPath)
        {
            #region // if file size is large than 1MB set it as back up
            if (File.Exists(_logPath))
            {
                // Create new FileInfo object and get the Length.
                FileInfo _fileInfo = new FileInfo(_logPath);

                // if file size is large than 1MB set it as back up
                if (_fileInfo.Length > 1000000 && log == "App started!")
                {
                    DateTime _lastWrite = _fileInfo.LastWriteTime;
                    string _backFile = _lastWrite.ToString("yyyyMMddhhmmss");
                    _backFile = mLogDirectory + "\\" + _backFile + ".bak";
                    try
                    {
                        //_fileInfo.CopyTo(_backFile);
                        //_fileInfo.Delete();
                        File.Move(_fileInfo.FullName, _backFile);
                    }
                    catch (Exception Ex)
                    {
                        //MessageBox.Show(
                        //    Ex.Message
                        //    , "Error"
                        //    , MessageBoxButtons.OK
                        //    , MessageBoxIcon.Exclamation);

                        Debug.Print(Ex.Message);
                        m_isAccessDenied = true;
                        return;
                    }
                }
            }
            #endregion

            if (!Directory.Exists(mLogDirectory))
            {
                Directory.CreateDirectory(mLogDirectory);
            }
            try
            {
                log = "\n" + timestamp + "  " + log;
                FileStream _stream = File.Open(_logPath, FileMode.Append);
                StreamWriter _writer = new StreamWriter(_stream);
                //if (Global.ServiceProvider.IsSimulate)
                //{
                Console.WriteLine(log);
                //}
                _writer.WriteLine(log);
                if (log.Contains("End")) _writer.WriteLine("");
                _writer.Close();
                _stream.Close();
            }
            catch (Exception Ex)
            {
                Debug.Print(Ex.Message);
                m_isAccessDenied = true;
                return;
            }

        }

        #endregion
        #endregion

        #region 导出excel
        /// <summary>
        /// 导出数据到excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lt"></param>
        public static void CreateAdvExcel<T>(ObservableCollection<T> lt)
        {
            StringBuilder builder = new StringBuilder();

            string name = System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss ffff") + ".xls";
            string path = "E:\\" + name;

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application(); //Excel应用程序
            _Workbook excelDoc = excelApp.Workbooks.Add(Missing.Value);  //Excel文档

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            Object nothing = Missing.Value;
            Object format = XlFileFormat.xlWorkbookDefault;
            excelDoc.SaveAs(path, nothing, nothing, nothing, nothing, nothing, XlSaveAsAccessMode.xlExclusive, nothing, nothing, nothing, nothing, nothing);

            excelDoc.Close(nothing, nothing, nothing);
            excelApp.Quit();
            int generation = System.GC.GetGeneration(excelApp);
            excelApp = null;


            //虽然用了_xlApp.Quit()，但由于是COM，并不能清除驻留在内存在的进程，每实例一次Excel则Excell进程多一个。
            //因此用垃圾回收，建议不要用进程的KILL()方法，否则可能会错杀无辜啊:)。
            System.GC.Collect(generation);


            if (lt == null || (0 == lt.Count))
            {
                return;
            }
            System.Reflection.PropertyInfo[] myPropertyInfo = lt.First().GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            int i = 0, j;
            for (i = 0, j = myPropertyInfo.Length; i < j; i++)
            {
                System.Reflection.PropertyInfo pi = myPropertyInfo[i];
                string headname = pi.Name;//单元格头部
                builder.Append(headname);
                builder.Append("\t");
            }
            builder.Append("\n");
            foreach (T t in lt)
            {
                if (lt == null)
                {
                    continue;
                }
                for (i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];
                    string str = string.Format("{0}", pi.GetValue(t, null)).Replace("\n", "");
                    if (str == "")
                    {
                        builder.Append("\t");
                    }
                    else
                    {
                        builder.Append(str + "\t");//横向跳到另一个单元格
                    }
                }
                builder.Append("\n");//换行
            }
            StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.GetEncoding("GB2312"));
            sw.Write(builder.ToString());//输出
            sw.Flush();
            sw.Close();


        }

        #endregion

        #region 导出DataGrid数据到CSV或者文本

        public static void ExportDataGrid(DataGrid dGrid)
        {
            SaveFileDialog objSFD = new SaveFileDialog() { DefaultExt = "csv", Filter = "CSV Files (*.csv)|*.csv|Excel XML (*.xml)|*.xml|All files (*.*)|*.*", FilterIndex = 1 };
            if (objSFD.ShowDialog() == true)
            {
                string strFormat = objSFD.SafeFileName.Substring(objSFD.SafeFileName.IndexOf('.') + 1).ToUpper();
                StringBuilder strBuilder = new StringBuilder();
                if (dGrid.ItemsSource == null) return;
                List<string> lstFields = new List<string>();
                if (dGrid.HeadersVisibility == DataGridHeadersVisibility.Column || dGrid.HeadersVisibility == DataGridHeadersVisibility.All)
                {
                    foreach (DataGridColumn dgcol in dGrid.Columns)
                        lstFields.Add(FormatField(dgcol.Header.ToString(), strFormat));
                    BuildStringOfRow(strBuilder, lstFields, strFormat);
                }
                foreach (object data in dGrid.ItemsSource)
                {
                    lstFields.Clear();
                    foreach (DataGridColumn col in dGrid.Columns)
                    {
                        string strValue = "";
                        Binding objBinding = null;
                        if (col is DataGridBoundColumn)
                            objBinding = (col as DataGridBoundColumn).Binding as Binding;
                        if (col is DataGridTemplateColumn)
                        {
                            //This is a template column... let us see the underlying dependency object
                            DependencyObject objDO = (col as DataGridTemplateColumn).CellTemplate.LoadContent();
                            FrameworkElement oFE = (FrameworkElement)objDO;
                            FieldInfo oFI = oFE.GetType().GetField("TextProperty");
                            if (oFI != null)
                            {
                                if (oFI.GetValue(null) != null)
                                {
                                    if (oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)) != null)
                                        objBinding = oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)).ParentBinding;
                                }
                            }
                        }
                        if (objBinding != null)
                        {
                            if (objBinding.Path.Path != "")
                            {
                                PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
                                if (pi != null) strValue = pi.GetValue(data, null).ToString();
                            }
                            if (objBinding.Converter != null)
                            {
                                if (strValue != "")
                                    strValue = objBinding.Converter.Convert(strValue, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                                else
                                    strValue = objBinding.Converter.Convert(data, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                            }
                        }
                        lstFields.Add(FormatField(strValue, strFormat));
                    }
                    BuildStringOfRow(strBuilder, lstFields, strFormat);
                }
                StreamWriter sw = new StreamWriter(objSFD.OpenFile(), Encoding.UTF8);
                if (strFormat == "XML")
                {
                    //Let us write the headers for the Excel XML
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    sw.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
                    sw.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                    sw.WriteLine("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">");
                    sw.WriteLine("<Author>Arasu Elango</Author>");
                    sw.WriteLine("<Created>" + DateTime.Now.ToLocalTime().ToLongDateString() + "</Created>");
                    sw.WriteLine("<LastSaved>" + DateTime.Now.ToLocalTime().ToLongDateString() + "</LastSaved>");
                    sw.WriteLine("<Company>Atom8 IT Solutions (P) Ltd.,</Company>");
                    sw.WriteLine("<Version>12.00</Version>");
                    sw.WriteLine("</DocumentProperties>");
                    sw.WriteLine("<Worksheet ss:Name=\"Silverlight Export\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                    sw.WriteLine("<Table>");
                }
                sw.Write(strBuilder.ToString());
                if (strFormat == "XML")
                {
                    sw.WriteLine("</Table>");
                    sw.WriteLine("</Worksheet>");
                    sw.WriteLine("</Workbook>");
                }
                sw.Close();
            }
        }
        private static void BuildStringOfRow(StringBuilder strBuilder, List<string> lstFields, string strFormat)
        {
            switch (strFormat)
            {
                case "XML":
                    strBuilder.AppendLine("<Row>");
                    strBuilder.AppendLine(String.Join("\r\n", lstFields.ToArray()));
                    strBuilder.AppendLine("</Row>");
                    break;
                case "CSV":
                    strBuilder.AppendLine(String.Join(",", lstFields.ToArray()));
                    break;
            }
        }
        private static string FormatField(string data, string format)
        {
            switch (format)
            {
                case "XML":
                    return String.Format("<Cell><Data ss:Type=\"String\">{0}</Data></Cell>", data);
                case "CSV":
                    return String.Format("\"{0}\"", data.Replace("\"", "\"\"\"").Replace("\n", "").Replace("\r", ""));
            }
            return data;
        }
        #endregion 导出DataGrid数据到Excel

    }
}
