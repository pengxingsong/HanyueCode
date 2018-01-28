using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 商品储位配置表
    /// </summary>
    [Description("商品储位配置表")]
    [PrimaryKey("ProductLocationID")]
    [TableName("Product_Location")]
    public class ProductLocationEntity : BaseEntity
    {
        /// <summary>
        /// 商品储位配置主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品储位配置主键")]
        public string ProductLocationID { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品主键")]
        public string ProductId { get; set; }

        /// <summary>
        /// 商户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商户主键")]
        public string MerchantId { get; set; }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库主键")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 库区主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("库区主键")]
        public string ZoneId { get; set; }

        /// <summary>
        /// 储位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("储位主键")]
        public string LocationId { get; set; }

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
        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ProductLocationID = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
