using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 订单明细表
    /// </summary>
    [Description("订单明细表")]
    [PrimaryKey("ItemId")]
    [TableName("SaleOrder_Item")]
    public class SaleOrderItemEntity : BaseEntity
    {
        /// <summary>
        /// 订单明细主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单明细主键")]
        public string ItemId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
        public string OrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品主键")]
        public string ProductId { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品编码")]
        public string Code { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 重量(Kg)
        /// </summary>
        /// <returns></returns>
        [DisplayName("重量(Kg)")]
        public decimal? Weight { get; set; }

        /// <summary>
        /// 体积(m3)
        /// </summary>
        /// <returns></returns>
        [DisplayName("体积(m3)")]
        public decimal? Volume { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        /// <returns></returns>
        [DisplayName("条码")]
        public string BarCode { get; set; }

        /// <summary>
        /// 定价
        /// </summary>
        /// <returns></returns>
        [DisplayName("定价")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        /// <returns></returns>
        [DisplayName("规格")]
        public string Specification { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("数量")]
        public int Qty { get; set; }


        /// <summary>
        /// 基本单位
        /// </summary>
        /// <returns></returns>
        [DisplayName("基本单位")]
        public string BaseUnit { get; set; }

        /// <summary>
        /// 已扫描数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("已扫描数量")]
        public int QtyScaned { get; set; }

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