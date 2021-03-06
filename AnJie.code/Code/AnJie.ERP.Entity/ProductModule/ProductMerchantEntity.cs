using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 商品商户对应表
    /// </summary>
    [Description("商品商户对应表")]
    [PrimaryKey("RelationId")]
    [TableName("Product_Merchant")]
    public class ProductMerchantEntity : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 商户仓库关系主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商户仓库关系主键")]
        public string RelationId { get; set; }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库主键")]
        public string ProductId { get; set; }

        /// <summary>
        /// 商户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商户主键")]
        public string MerchantId { get; set; }

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

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.RelationId = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RelationId = KeyValue;
                                            }
        #endregion
    }
}