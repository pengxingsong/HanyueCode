using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnJie.ERP.ViewModel.MerchantModule
{
    public class MerchantShipTemplateViewModel
    {

        /// <summary>
        /// 模板Id
        /// </summary>
        /// <returns></returns>
        public string TemplateId { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        public string MerchantId { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        public string MerchantName { get; set; }

        /// <summary>
        /// 订单发货仓
        /// </summary>
        /// <returns></returns>
        public string WarehouseId { get; set; }

        /// <summary>
        /// 订单发货仓
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        /// <returns></returns>
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        /// <returns></returns>
        public string ShipTypeName { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        /// <returns></returns>
        public string TemplateName { get; set; }

        /// <summary>
        /// 起步重量
        /// </summary>
        /// <returns></returns>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 加价重量
        /// </summary>
        /// <returns></returns>
        public decimal? AddWeight { get; set; }

        /// <summary>
        /// 默认起步价
        /// </summary>
        /// <returns></returns>
        public decimal? Price { get; set; }

        /// <summary>
        /// 默认加价
        /// </summary>
        /// <returns></returns>
        public decimal? AddPrice { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        public int? Enabled { get; set; }

        /// <summary>
        /// 是否默认模板
        /// </summary>
        /// <returns></returns>
        public int? IsDefault { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        public int? SortCode { get; set; }
    }
}