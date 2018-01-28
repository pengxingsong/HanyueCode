using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AnJie.ERP.Business
{
    public enum PickItemStatus
    {
        /// <summary>
        /// 初始
        /// </summary>
        [Description("初始")]
        Initial = 0,

        /// <summary>
        /// 开始拣货
        /// </summary>
        [Description("开始拣货")]
        Picking = 1,

        /// <summary>
        /// 拣货完成
        /// </summary>
        [Description("拣货完成")]
        Picked = 2
    }

    public enum PickMasterStatus
    {
        /// <summary>
        /// 初始
        /// </summary>
        [Description("初始")]
        Initial = 0,

        /// <summary>
        /// 开始拣货
        /// </summary>
        [Description("开始拣货")]
        Picking = 1,

        /// <summary>
        /// 拣货完成
        /// </summary>
        [Description("拣货完成")]
        Picked = 2
    }
}
