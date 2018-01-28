using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// SaleOrder_ImportFile
    /// </summary>
    [Description("SaleOrder_ImportFile")]
    [PrimaryKey("FileId")]
    [TableName("SaleOrder_ImportFile")]
    public class SaleOrderImportFileEntity : BaseEntity
    {
        /// <summary>
        /// 订单主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单主键")]
        public string FileId { get; set; }

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
        /// MerchantMallId
        /// </summary>
        /// <returns></returns>
        [DisplayName("MerchantMallId")]
        public string MerchantMallId { get; set; }

        /// <summary>
        /// FileName
        /// </summary>
        /// <returns></returns>
        [DisplayName("FileName")]
        public string FileName { get; set; }

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
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.FileId = CommonHelper.GetGuid;
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
            this.FileId = keyValue;
        }
    }
}