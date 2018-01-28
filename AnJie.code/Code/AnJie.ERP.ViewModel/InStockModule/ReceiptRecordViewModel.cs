using System;
using System.ComponentModel;

namespace AnJie.ERP.ViewModel.InStockModule
{
    /// <summary>
    /// 收货单明细表
    /// </summary>
    public class ReceiptRecordViewModel
    {

        /// <summary>
        /// 
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReceiptId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        public string ReceiptItemId { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        public string ProductId { get; set; }

        /// <summary>
        /// 储位主键
        /// </summary>
        /// <returns></returns>
        public string LocationId { get; set; }

        /// <summary>
        /// LocationCode
        /// </summary>
        /// <returns></returns>
        public string LocationCode { get; set; }

        /// <summary>
        /// ReceivedQty
        /// </summary>
        /// <returns></returns>
        public int ReceivedQty { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <returns></returns>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }

    }
}