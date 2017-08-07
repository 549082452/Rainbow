using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rainbow.Utility.Interface;
using Rainbow.Utility.Communication;
using Rainbow.Utility.Model;

namespace Rainbow.Utility
{
    public class GlobalProvider
    {
        #region 全局常量
        public const string FormatDouble = "f2";
        public const double DiagnosticsWave = 350;
        #endregion

        #region 全局变量
        public static MoveEnum CurrentMove;
        public static int MarkOrder;
        #endregion

        #region MeasureUserControl
        public static IMeasureUser MeasureUserCtrl
        {
            get;
            set;
        }
        public static OmniProvider OmniProvider = new OmniProvider();
        #endregion

        /// <summary>
        /// 染料列表
        /// </summary>
        public static List<DyeModel> DyeList { get; set; }

        #region Log
        public static string CurrentClass
        {
            get;
            set;
        }
        #endregion
    }
}
