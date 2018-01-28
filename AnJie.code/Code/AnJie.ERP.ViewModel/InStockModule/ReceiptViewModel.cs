using System;

namespace AnJie.ERP.ViewModel.InStockModule
{
    /// <summary>
    /// 收货单
    /// </summary>
    public class ReceiptViewModel
    {
        /// <summary>
        /// 订单主键
        /// </summary>
        public string ReceiptId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string ReceiptNo { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// 收货类型
        /// </summary>
        public int? ReceiptType { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        public string MerchantName { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 审核状态码
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 制单人主键
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 制单时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 修改人主键
        /// </summary>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLocked { get; set; }
    }
}