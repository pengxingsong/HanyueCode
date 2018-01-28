using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 收货单主表
    /// </summary>
    [Description("收货单主表")]
    [PrimaryKey("ReceiptId")]
    [TableName("Receipt")]
    public class ReceiptEntity : BaseEntity
    {
        /// <summary>
        /// 订单主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单主键")]
        public string ReceiptId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
        public string ReceiptNo { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单日期")]
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// 收货类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货类型")]
        public int? ReceiptType { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货仓库")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        [DisplayName("所属商户")]
        public string MerchantId { get; set; }

        /// <summary>
        /// 所属店铺
        /// </summary>
        /// <returns></returns>
        [DisplayName("所属店铺")]
        public string MerchantMallId { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("来源单号")]
        public string SourceNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("锁定")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 制单人主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("制单人主键")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        /// <returns></returns>
        [DisplayName("制单人")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 制单时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("制单时间")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 修改人主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人主键")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ReceiptId = CommonHelper.GetGuid;
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
            this.ReceiptId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}