using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.UIControl.Chart
{
    /// <summary>
    /// 坐标类型枚举
    /// </summary>
    public enum CoordinateInfoEnum
    {
        /// <summary>
        ///  X-Y
        /// </summary>
        XY = 0,
        /// <summary>
        /// Log(X)-Log(Y)
        /// </summary>
        LogXLogY = 1,
        /// <summary>
        /// Log(X)-Y
        /// </summary>
        LogXY = 2,
        /// <summary>
        /// X-Log(Y)
        /// </summary>
        XLogY = 3,
        /// <summary>
        /// X-Logit(Y)
        /// </summary>
        XLogitY = 4,
        /// <summary>
        /// Log(X)-Logit(Y)
        /// </summary>
        LogXLogitY = 5,
        /// <summary>
        /// X-B/B0(%)
        /// </summary>
        XBB0 = 6,
        /// <summary>
        /// Log(X)-B/B0(%)
        /// </summary>
        LogXBB0 = 7
    }


    /// <summary>
    /// 坐标轴名称(X,Y)
    /// </summary>
    public enum AxisTypeName
    {
        /// <summary>
        /// 主 X 轴。
        /// </summary>
        X = 0,
        /// <summary>
        ///  主 Y 轴。
        /// </summary>
        Y = 1,
        /// <summary>
        /// 辅助X轴。
        /// </summary>
        X2 = 2,
        /// <summary>
        /// 辅助 Y 轴。
        /// </summary>
        Y2 = 3,
    }
}
