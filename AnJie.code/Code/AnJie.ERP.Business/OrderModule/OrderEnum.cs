using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        Canceled = -2,

        /// <summary>
        /// 缺货
        /// </summary>
        [Description("缺货")]
        OutOfStock = -1,

        /// <summary>
        /// 初始
        /// </summary>
        [Description("初始")]
        Initial = 0,

        /// <summary>
        /// 待审核
        /// </summary>
        [Description("待审核")]
        WaitAudit = 1,

        /// <summary>
        /// 已审核待发货
        /// </summary>
        [Description("待发货")]
        WaitConfirm = 2,

        /// <summary>
        /// 已发货待拣货
        /// </summary>
        [Description("待拣货")]
        WaitPick = 4,

        /// <summary>
        /// 已拣货待出库
        /// </summary>
        [Description("待出库")]
        WaitOutStock = 6,

        /// <summary>
        /// 已出库
        /// </summary>
        [Description("已出库")]
        OutStock = 8,

        /// <summary>
        /// 已交接
        /// </summary>
        [Description("已交接")]
        Handover = 10 
    }

    /// <summary>
    /// 订单出库状态
    /// </summary>
    public enum OutStockStatus
    {
        /// <summary>
        /// 出库取消
        /// </summary>
        [Description("出库取消")]
        Cancel = -1,

        /// <summary>
        /// 初始
        /// </summary>
        [Description("初始")]
        Initial = 0,

        /// <summary>
        /// 已配货
        /// </summary>
        [Description("已配货")]
        Allocated = 1,

        /// <summary>
        /// 开始拣货
        /// </summary>
        [Description("开始拣货")]
        Picking = 2,

        /// <summary>
        /// 开始总拣
        /// </summary>
        [Description("开始总拣")]
        TotalPicking = 4,

        /// <summary>
        /// 拣货完成
        /// </summary>
        [Description("拣货完成")]
        PickFinished = 6,

        /// <summary>
        /// 打包完成
        /// </summary>
        [Description("已打包")]
        Packaged = 8
    }
}
