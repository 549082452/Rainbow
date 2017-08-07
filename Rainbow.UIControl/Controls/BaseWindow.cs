using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;
using Global;

namespace ClipperBaseUI.Controls
{
    public class BaseWindow : Window
    {
        private ResourceDictionary m_WindowResouce = new ResourceDictionary();
        private ControlTemplate m_ControlTemplate;
        BaseButton btnSetting;
        protected bool? m_DialogResult=null;
        #region 属性
        /// <summary>
        /// 获取设置按钮
        /// </summary>
        protected BaseButton BtnSetting
        {
            get
            {
                return btnSetting;
            }
        }
        Visibility showMax;
        /// <summary>
        /// 是否显示最大化按钮
        /// </summary>
        public Visibility ShowMax
        {
            get
            {
                return showMax;
            }
            set
            {
                showMax = value;
            }
        }
        Visibility showMin;
        /// <summary>
        /// 是否显示最小化按钮
        /// </summary>
        public Visibility ShowMin
        {
            get
            {
                return showMin;
            }
            set
            {
                showMin = value;
            }
        }
        Visibility showClose;
        public Visibility ShowClose
        {
            get
            {
                return showClose;
            }
            set
            {
                showClose = value;
            }
        }
        Visibility showSetting = Visibility.Collapsed;
        /// <summary>
        /// 是否显示设置
        /// </summary>
        public Visibility ShowSetting
        {
            set
            {
                showSetting = value;
            }
            get
            {
                return showSetting;
            }
        }
        Visibility showHelp = Visibility.Collapsed;
        /// <summary>
        /// 是否显示帮助
        /// </summary>
        public Visibility ShowHelp
        {
            get
            {
                return showHelp;
            }
            set
            {
                showHelp = value;
            }
        }

        private ServiceProvider _serviceProvider;
        /// <summary>
        /// 参数传递
        /// </summary>
        public ServiceProvider BaseServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = new ServiceProvider();
                }
                return _serviceProvider;
            }
            set
            {
                _serviceProvider = value;
            }
        }
        /// <summary>
        /// 窗口返回值
        /// </summary>
        public new bool? DialogResult
        {
            get
            {
                return m_DialogResult;
            }
        }
        #endregion

        public BaseWindow()
        {
            this.DefaultStyleKey = typeof(BaseWindow);

            m_WindowResouce.Source = new Uri("ClipperBaseUI;component/Themes/BaseWindow.xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(m_WindowResouce);
            this.Style=(Style)m_WindowResouce["WindowStyle"];
            m_ControlTemplate = (ControlTemplate)(this.Style.Setters[3] as Setter).Value;
           
            this.Loaded += delegate
            {
                InitializeEvent();
            };
        }

        private void InitializeEvent()
        {
            Border winHead = (Border)m_ControlTemplate.FindName("WinHead", this);
            winHead.MouseLeftButtonDown += new MouseButtonEventHandler(winHead_MouseLeftButtonDown);

            BaseButton btnMin = (BaseButton)m_ControlTemplate.FindName("btnMin", this);
            btnMin.Visibility = showMin;
            btnMin.Click += delegate
            {
                this.WindowState = WindowState.Minimized;
            };

            BaseButton btnMax = (BaseButton)m_ControlTemplate.FindName("btnMax", this);
            btnMax.Visibility = showMax;
            btnMax.Click += delegate
            {
                this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
                //btnMax.ToolTip = ((this.WindowState == WindowState.Normal) ? "最大化" : "还原");
            };

            BaseButton btnClose = (BaseButton)m_ControlTemplate.FindName("btnClose", this);
            btnClose.Visibility = showClose;
            btnClose.Click += delegate
            {
                this.Close();
            };

            btnSetting = (BaseButton)m_ControlTemplate.FindName("btnSetting", this);
            btnSetting.Visibility = showSetting;
            btnSetting.Click += delegate
            {
                Setting();
            };

            
        }

        void winHead_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public virtual void Init(ServiceProvider service)
        {
            if (service != null)
            {
                _serviceProvider = service.CreateProvider(service);
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        public virtual void Setting()
        {

        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        public virtual void Help()
        {

        }
    }
}
