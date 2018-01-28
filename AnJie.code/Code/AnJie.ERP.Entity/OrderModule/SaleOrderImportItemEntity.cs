using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// SaleOrder_ImportItem
    /// </summary>
    [Description("SaleOrder_ImportItem")]
    [PrimaryKey("ItemId")]
    [TableName("SaleOrder_ImportItem")]
    public class SaleOrderImportItemEntity : BaseEntity
    {
        /// <summary>
        /// 订单主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单主键")]
        public string ItemId { get; set; }

        /// <summary>
        /// FileId
        /// </summary>
        /// <returns></returns>
        [DisplayName("FileId")]
        public string FileId { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品编码")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 外部订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("外部订单号")]
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// 单据日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("单据日期")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        [DisplayName("联系人")]
        public string ReceiveContact { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("固定电话")]
        public string ReceivePhone { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("联系电话")]
        public string ReceiveCellPhone { get; set; }

        /// <summary>
        /// 物流公司
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流公司")]
        public string ShipTypeName { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流单号")]
        public string ExpressNum { get; set; }

        /// <summary>
        /// 买家省份
        /// </summary>
        /// <returns></returns>
        [DisplayName("买家省份")]
        public string Province { get; set; }

        /// <summary>
        /// 买家城市
        /// </summary>
        /// <returns></returns>
        [DisplayName("买家城市")]
        public string City { get; set; }

        /// <summary>
        /// 买家地区
        /// </summary>
        /// <returns></returns>
        [DisplayName("买家地区")]
        public string County { get; set; }

        /// <summary>
        /// 送货地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("送货地址")]
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// 卖家留言
        /// </summary>
        /// <returns></returns>
        [DisplayName("卖家留言")]
        public string SellerNote { get; set; }

        /// <summary>
        /// 买家留言
        /// </summary>
        /// <returns></returns>
        [DisplayName("买家留言")]
        public string BuyerNote { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品数量")]
        public int Qty { get; set; }


        /// <summary>
        /// 收货人邮编
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货人邮编")]
        public string ReceiveZip { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ItemId = CommonHelper.GetGuid;
        }

        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ItemId = keyValue;
        }
    }
}