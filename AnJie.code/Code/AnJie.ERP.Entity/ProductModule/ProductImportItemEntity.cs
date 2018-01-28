using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 商品导入明细
    /// </summary>
    [Description("商品导入明细")]
    [PrimaryKey("ProductId")]
    [TableName("Product_ImportItem")]
    public class ProductImportItemEntity : BaseEntity
    {
        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品主键")]
        public string ProductId { get; set; }

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
        /// 商品名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品简称
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品简称")]
        public string BriefName { get; set; }

        /// <summary>
        /// 重量(Kg)
        /// </summary>
        /// <returns></returns>
        [DisplayName("重量(Kg)")]
        public decimal? Weight { get; set; }

        /// <summary>
        /// 体积(m³)
        /// </summary>
        /// <returns></returns>
        [DisplayName("体积(m³)")]
        public decimal? Volume { get; set; }

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
        /// 品牌
        /// </summary>
        /// <returns></returns>
        [DisplayName("品牌")]
        public string Brand { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        /// <returns></returns>
        [DisplayName("条码")]
        public string BarCode { get; set; }

        /// <summary>
        /// 基本单位
        /// </summary>
        /// <returns></returns>
        [DisplayName("基本单位")]
        public string BaseUnit { get; set; }

        /// <summary>
        /// 宽度(Cm)
        /// </summary>
        /// <returns></returns>
        [DisplayName("长度(Cm)")]
        public decimal? Width { get; set; }

        /// <summary>
        /// 高度(Cm)
        /// </summary>
        /// <returns></returns>
        [DisplayName("高度(Cm)")]
        public decimal? Height { get; set; }

        /// <summary>
        /// 长度(CM)
        /// </summary>
        /// <returns></returns>
        [DisplayName("长度(CM)")]
        public decimal? Long { get; set; }

        /// <summary>
        /// 批号管控
        /// </summary>
        /// <returns></returns>
        [DisplayName("批号管控")]
        public int? IsLotControl { get; set; }

        /// <summary>
        /// 是否强制扫描
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否强制扫描")]
        public int? IsForcedScan { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ProductId = CommonHelper.GetGuid;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ProductId = keyValue;
        }
        #endregion
    }
}