using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Business.InStockModule
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum ReceiptStatus
    {
        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        Canceled = -1,

        /// <summary>
        /// 初始
        /// </summary>
        [Description("初始")]
        Initial = 0,

        /// <summary>
        /// 已审核
        /// </summary>
        [Description("已审核")]
        Audited = 1,

        /// <summary>
        /// 收货中
        /// </summary>
        [Description("收货中")]
        Receiving = 2,

        /// <summary>
        /// 收货完成
        /// </summary>
        [Description("收货完成")]
        Received = 3
    }
}
