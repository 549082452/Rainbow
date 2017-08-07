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
using Clipper.BLL;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ClipperBaseUI.Controls
{
    /// <summary>
    /// DetectItemSet.xaml 的交互逻辑
    /// </summary>
    public partial class DetectItemSet : UserControl
    {
       
        #region 属性
        public static readonly DependencyProperty SelectedItemProperty;
        public static readonly DependencyProperty CurrentItemProperty;
        public event SelectionChangedEventHandler SelectionChangedEvent;
        bool canSelecGroup = true;
        ObservableCollection<Clipper.Model.DetectItems> mDetectItems;
        /// <summary>
        /// 所有选中的项
        /// </summary>
        public ObservableCollection<Clipper.Model.DetectItems> SelectedItem
        {
            get
            {
                
                return base.GetValue(DetectItemSet.SelectedItemProperty) as ObservableCollection<Clipper.Model.DetectItems>;
            }
            set
            {
                base.SetValue(DetectItemSet.SelectedItemProperty, value);
            }
        }
        /// <summary>
        /// 当前选中项
        /// </summary>
        public Clipper.Model.DetectItems CurrentItem
        {
            get
            {
                return base.GetValue(DetectItemSet.CurrentItemProperty) as Clipper.Model.DetectItems;
            }
            set
            {
                base.SetValue(DetectItemSet.CurrentItemProperty, value);
            }
        }

        /// <summary>
        /// 是否使用全选功能
        /// </summary>
        public bool CanSelectGroup
        {
            get
            {
                return canSelecGroup;
            }
            set
            {
                canSelecGroup = value;
            }
        }
        #endregion

        #region 初始化

        public DetectItemSet()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!CanSelectGroup)
            {
                lsvDetectItem.GroupStyle.Clear();
                GroupStyle groupStyle = (GroupStyle)this.Resources["groupStyle"];
                lsvDetectItem.GroupStyle.Add(groupStyle);
                
            }
        }
        /// <summary>
        /// 异步加载检测项
        /// </summary>
        public void DataBind()
        {
            DetectItems itemsBll = new DetectItems();
            BackgroundWorker backWorker = new BackgroundWorker();
            backWorker.DoWork += (ss, ee) =>
                {
                     mDetectItems = itemsBll.GetSelectItemsList();
                };
            backWorker.RunWorkerCompleted += (ss, ee) =>
                {
                    ICollectionView mViewCollection = CollectionViewSource.GetDefaultView(mDetectItems);
                    mViewCollection.GroupDescriptions.Add(new PropertyGroupDescription("DetectVariety.typeName"));
                    lsvDetectItem.ItemsSource = mViewCollection;
                };
            backWorker.RunWorkerAsync();
        }
        /// <summary>
        /// 同步加载检测项
        /// </summary>
        public void ItemDataBind()
        {
            ICollectionView mViewCollection = CollectionViewSource.GetDefaultView(mDetectItems);
            mViewCollection.GroupDescriptions.Add(new PropertyGroupDescription("DetectVariety.typeName"));
            lsvDetectItem.ItemsSource = mViewCollection;
        }
        /// <summary>
        /// 获取检测项信息
        /// </summary>
        public void GetItemData()
        {
            DetectItems itemsBll = new DetectItems();
            mDetectItems = itemsBll.GetSelectItemsList();
        }
        static DetectItemSet()
		{
            DetectItemSet.SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(ObservableCollection<Clipper.Model.DetectItems>), typeof(DetectItemSet), new PropertyMetadata(null));
            DetectItemSet.CurrentItemProperty = DependencyProperty.Register("CurrentItem", typeof(Clipper.Model.DetectItems), typeof(DetectItemSet), new PropertyMetadata(null));
		}
        #endregion

        #region 事件
        //选中检测项事件
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            Button _btn=sender as Button;
            ContentPresenter _contentPresenter = _btn.Content as ContentPresenter;
            Clipper.Model.DetectItems model =_contentPresenter.Content as Clipper.Model.DetectItems;
            if (model != null)
            {
                if (SelectedItem == null)
                {
                    SelectedItem = new ObservableCollection<Clipper.Model.DetectItems>();
                }
                //if (!SelectedItem.Contains(model))
                //{
                SelectedItem.Add(model);
                //}
                CurrentItem = model;
                if (SelectionChangedEvent != null)
                {
                    SelectionChangedEvent.Invoke(sender, null);
                }
            }
        }
        //选中检测项事件
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox _listview = sender as ListBox;
            Clipper.Model.DetectItems model = _listview.SelectedItem as Clipper.Model.DetectItems;
            if (model != null)
            {
                if (SelectedItem == null)
                {
                    SelectedItem = new ObservableCollection<Clipper.Model.DetectItems>();
                }
                //if (!SelectedItem.Contains(model))
                //{
                    SelectedItem.Add(model);
                //}
                CurrentItem = model;
                if (SelectionChangedEvent != null)
                {
                    SelectionChangedEvent.Invoke(sender, e);
                }
            }
        }

        /// <summary>
        /// 全选一组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btnGroup = sender as Button;
            //IEnumerable<Clipper.Model.DetectItems> items= mDetectItems.Where<Clipper.Model.DetectItems>(x => x.DetectVariety.typeName == btnGroup.Content.ToString());
            foreach (Clipper.Model.DetectItems model in mDetectItems)
            {
                if (model.DetectVariety.typeName == btnGroup.Content.ToString())
                {
                    if (SelectedItem == null)
                    {
                        SelectedItem = new ObservableCollection<Clipper.Model.DetectItems>();
                    }
                    //if (!SelectedItem.Contains(model))
                    //{
                        SelectedItem.Add(model);
                    //}
                    CurrentItem = model;
                    if (SelectionChangedEvent != null)
                    {
                        SelectionChangedEvent.Invoke(sender, null);
                    }
                }
            }
        }
        #endregion 

    }
}
