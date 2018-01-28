using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AnJie.ERP.Business
{
    public enum YesNoStatus
    {
        /// <summary>
        /// 否
        /// </summary>
        [Description("否")] No = 0,

        /// <summary>
        /// 是
        /// </summary>
        [Description("是")] Yes = 1
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum PrintStatus
    {
        /// <summary>
        /// 待打印
        /// </summary>
        [Description("待打印")]
        WaitPrint = 0,

        /// <summary>
        /// 打印中
        /// </summary>
        [Description("打印中")]
        Printing = 1,

        /// <summary>
        /// 已打印
        /// </summary>
        [Description("已打印")]
        Printed = 2,
    }
}
