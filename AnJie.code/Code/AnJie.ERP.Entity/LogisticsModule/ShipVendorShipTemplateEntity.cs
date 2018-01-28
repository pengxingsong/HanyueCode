using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 配送商运费结算模板
    /// </summary>
    [Description("配送商运费结算模板")]
    [PrimaryKey("TemplateId")]
    [TableName("ShipVendor_ShipTemplate")]
    public class ShipVendorShipTemplateEntity : BaseEntity
    {
        /// <summary>
        /// 模板Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("模板Id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        [DisplayName("所属商户")]
        public string ShipVendorId { get; set; }

        /// <summary>
        /// 订单发货仓
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单发货仓")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流方式")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("模板名称")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 起步重量
        /// </summary>
        /// <returns></returns>
        [DisplayName("起步重量")]
        public decimal? Weight { get; set; }

        /// <summary>
        /// 加价重量
        /// </summary>
        /// <returns></returns>
        [DisplayName("加价重量")]
        public decimal? AddWeight { get; set; }

        /// <summary>
        /// 默认起步价
        /// </summary>
        /// <returns></returns>
        [DisplayName("默认起步价")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 默认加价
        /// </summary>
        /// <returns></returns>
        [DisplayName("默认加价")]
        public decimal? AddPrice { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效")]
        public int? Enabled { get; set; }

        /// <summary>
        /// 是否默认模板
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否默认模板")]
        public int? IsDefault { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [DisplayName("排序码")]
        public int? SortCode { get; set; }

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
            this.TemplateId = CommonHelper.GetGuid;
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
            this.TemplateId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}