using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnJie.ERP.ViewModel.OutStockModule
{
    public class PrintBatchItemViewModel
    {
        /// <summary>
        /// 批次打印主键
        /// </summary>
        /// <returns></returns>
        public string ItemId { get; set; }

        /// <summary>
        /// 批次打印主键
        /// </summary>
        /// <returns></returns>
        public string BatchId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        public string OrderNo { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressNum { get; set; }
    }
}