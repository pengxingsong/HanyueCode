using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 商品信息表
    /// </summary>
    [Description("商品信息表")]
    [PrimaryKey("ProductId")]
    [TableName("Product")]
    public class ProductEntity : BaseEntity
    {
        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品主键")]
        public string ProductId { get; set; }

        ///// <summary>
        ///// 商户主键
        ///// </summary>
        ///// <returns></returns>
        //[DisplayName("商户主键")]
        //public string MerchantId { get; set; }

        /// <summary>
        /// 分类主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("分类主键")]
        public string CategoryId { get; set; }

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

        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效")]
        public int? Enabled { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [DisplayName("排序码")]
        public int? SortCode { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        [DisplayName("删除标记")]
        public int? DeleteMark { get; set; }

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

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ProductId = CommonHelper.GetGuid;
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
            this.ProductId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}