using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 库存流水
    /// </summary>
    [Description("库存流水")]
    [PrimaryKey("TransactionId")]
    [TableName("Inventory_Transaction")]
    public class InventoryTransactionEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string TransactionId { get; set; }

        /// <summary>
        /// 交易种类（枚举：收货、拣货、...）
        /// </summary>
        /// <returns></returns>
        [DisplayName("交易种类（枚举：收货、拣货、...）")]
        public int Type { get; set; }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库主键")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品主键")]
        public string ProductId { get; set; }

        /// <summary>
        /// 来源商户
        /// </summary>
        /// <returns></returns>
        [DisplayName("来源商户")]
        public string MerchantFrom { get; set; }

        /// <summary>
        /// 目的商户
        /// </summary>
        /// <returns></returns>
        [DisplayName("目的商户")]
        public string MerchantTo { get; set; }

        /// <summary>
        /// 交易数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("交易数量")]
        public int Qty { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("来源单号")]
        public string SourceNo { get; set; }

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
            this.TransactionId = CommonHelper.GetGuid;
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
            this.TransactionId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}
