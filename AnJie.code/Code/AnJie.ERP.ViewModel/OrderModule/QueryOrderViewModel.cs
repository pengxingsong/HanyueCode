using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnJie.ERP.ViewModel.OrderModule
{
    public class QueryOrderViewModel
    {
        /// <summary>
        /// 查询类型（All所有，PrintBatch按打印批次，QueryByPrintStatus按状态查询，Suspend挂起订单）
        /// </summary>
        public string QueryType { get; set; }

        /// <summary>
        /// 打印批号（QueryType=PrintBatch）
        /// </summary>
        public string PrintBatchId { get; set; }

        /// <summary>
        /// 商户
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 外部单号
        /// </summary>
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 订单状态(打印使用，多个状态)
        /// </summary>
        public bool StatusWithPrint { get; set; }

        /// <summary>
        /// 打印状态
        /// </summary>
        public int? PrintStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReceiveContact { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReceivePhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExpressNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        /// <returns></returns>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }
        
        /// <summary>
        /// 县/区
        /// </summary>
        /// <returns></returns>
        public string County { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        /// <returns></returns>
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// 锁单分钟
        /// </summary>
        public int? LockMinute { get; set; }
    }
}