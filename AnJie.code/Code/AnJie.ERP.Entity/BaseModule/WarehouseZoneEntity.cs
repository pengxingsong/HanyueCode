using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 库存信息
    /// </summary>
    [Description("库存信息")]
    [PrimaryKey("ZoneId")]
    [TableName("Warehouse_Zone")]
    public class WarehouseZoneEntity : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 库区主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("库区主键")]
        public string ZoneId { get; set; }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库主键")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        /// <returns></returns>
        [DisplayName("库区编码")]
        public string ZoneCode { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("库区名称")]
        public string ZoneName { get; set; }

        /// <summary>
        /// 库区类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("库区类型")]
        public int? ZoneType { get; set; }

        /// <summary>
        /// 移入暂存库位
        /// </summary>
        /// <returns></returns>
        [DisplayName("移入暂存库位")]
        public string InLoc { get; set; }

        /// <summary>
        /// 移出暂存库位
        /// </summary>
        /// <returns></returns>
        [DisplayName("移出暂存库位")]
        public string OutLoc { get; set; }

        /// <summary>
        /// 盘点方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("盘点方式")]
        public int? CheckMethod { get; set; }

        /// <summary>
        /// 拣选方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("拣选方式")]
        public int? PickMethod { get; set; }

        /// <summary>
        /// 是否总拣
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否总拣")]
        public int? IsCollect { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否启用")]
        public int? Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Comments { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        [DisplayName("排序字段")]
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

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ZoneId = CommonHelper.GetGuid;
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
            this.ZoneId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}