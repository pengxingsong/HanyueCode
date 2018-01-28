using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    [Description("仓库信息")]
    [PrimaryKey("WarehouseId")]
    [TableName("Warehouse")]
    public class WarehouseEntity : BaseEntity
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库编号")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// WarehouseCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("WarehouseCode")]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库名称")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 公司主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("公司主键")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 仓库地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库地址")]
        public string Address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        [DisplayName("联系人")]
        public string Contact { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("联系电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public int? Status { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        /// <returns></returns>
        [DisplayName("邮编")]
        public string PostalCode { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货地址")]
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// 收货联系人
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货联系人")]
        public string ReceiveContact { get; set; }

        /// <summary>
        /// 收货联系人电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货联系人电话")]
        public string ReceivePhone { get; set; }

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
        /// 默认收货储位
        /// </summary>
        /// <returns></returns>
        [DisplayName("默认收货储位")]
        public string ReceiptLocationId { get; set; }

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
            this.WarehouseId = CommonHelper.GetGuid;
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
            this.WarehouseId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}