using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// Receipt_Record
    /// </summary>
    [Description("收货记录")]
    [PrimaryKey("ItemId")]
    [TableName("Receipt_Record")]
    public class ReceiptRecordEntity : BaseEntity
    {
        /// <summary>
        /// 订单明细主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单明细主键")]
        public string ItemId { get; set; }

        /// <summary>
        /// 收货单主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货单主键")]
        public string ReceiptId { get; set; }

        /// <summary>
        /// 收货单明细主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货单明细主键")]
        public string ReceiptItemId { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品主键")]
        public string ProductId { get; set; }

        /// <summary>
        /// 储位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("储位主键")]
        public string LocationId { get; set; }

        /// <summary>
        /// LocationCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("LocationCode")]
        public string LocationCode { get; set; }

        /// <summary>
        /// ReceivedQty
        /// </summary>
        /// <returns></returns>
        [DisplayName("ReceivedQty")]
        public int ReceivedQty { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <returns></returns>
        [DisplayName("Status")]
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建用户主键")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建用户")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改用户主键")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改用户")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ItemId = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ItemId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}