using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnJie.ERP.ViewModel.InventoryModule
{
    public class InventoryViewModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string InventoryId { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        /// <returns></returns>
        public string WarehouseId { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        public string ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        public string ProductName { get; set; }

        /// <summary>
        /// 商户编号
        /// </summary>
        /// <returns></returns>
        public string MerchantId { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        /// <returns></returns>
        public string MerchantName { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        /// <returns></returns>
        public int QtyOnHand { get; set; }

        /// <summary>
        /// 已分配数量
        /// </summary>
        /// <returns></returns>
        public int QtyAllocated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyDate { get; set; }
    }
}