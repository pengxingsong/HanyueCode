using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 商户运费结算模板明细
    /// </summary>
    [Description("商户运费结算模板明细")]
    [PrimaryKey("ItemId")]
    [TableName("Merchant_ShipTemplateItem")]
    public class MerchantShipTemplateItemEntity : BaseEntity
    {
        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        [DisplayName("所属商户")]
        public string ItemId { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("模板Id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 省主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("省主键")]
        public string ProvinceId { get; set; }

        /// <summary>
        /// 市主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("市主键")]
        public string CityId { get; set; }

        /// <summary>
        /// 县/区主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("县/区主键")]
        public string CountyId { get; set; }

        /// <summary>
        /// 起步价
        /// </summary>
        /// <returns></returns>
        [DisplayName("起步价")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 加价
        /// </summary>
        /// <returns></returns>
        [DisplayName("加价")]
        public decimal? AddPrice { get; set; }

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