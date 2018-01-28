using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 商户库存交易种类
    /// </summary>
    public enum InventoryTransactionType
    {
        /// <summary>
        /// 收货
        /// </summary>
        [Description("收货")]
        Receive = 1,
        /// <summary>
        /// 拣货
        /// </summary>
        [Description("拣货")]
        Pick = 2,
        /// <summary>
        /// 出库
        /// </summary>
        [Description("出库")]
        OutStock = 3,
        /// <summary>
        /// 取消出库
        /// </summary>
        [Description("取消出库")]
        CancelOutStock = 4,
        /// <summary>
        /// 取消收货
        /// </summary>
        [Description("取消收货")]
        CancelReceive = 5,
    }

    /// <summary>
    /// 库位库存交易种类
    /// </summary>
    public enum InventoryLocationTransactionType
    {
        /// <summary>
        /// 收货
        /// </summary>
        [Description("收货")]
        Receive = 1,
        /// <summary>
        /// 拣货
        /// </summary>
        [Description("拣货")]
        Picked = 2,
        /// <summary>
        /// 出库
        /// </summary>
        [Description("出库")]
        OutStock = 3,
        /// <summary>
        /// 打包
        /// </summary>
        [Description("打包")]
        Package = 6,
        /// <summary>
        /// 取消出库
        /// </summary>
        [Description("取消出库")]
        CancelOutStock = 4,
        /// <summary>
        /// 取消收货
        /// </summary>
        [Description("取消收货")]
        CancelReceive = 5,

    }
}
