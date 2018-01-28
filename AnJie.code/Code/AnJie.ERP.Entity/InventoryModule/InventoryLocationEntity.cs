using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 库位库存表
    /// </summary>
    [Description("Inventory")]
    [PrimaryKey("InventoryId")]
    [TableName("Inventory_Location")]
    public class InventoryLocationEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string InventoryId { get; set; }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库主键")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商品主键")]
        public string ProductId { get; set; }

        /// <summary>
        /// 储位编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("储位编号")]
        public string LocationCode { get; set; }

        /// <summary>
        /// 在库数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("在库数量")]
        public int QtyOnHand { get; set; }

        /// <summary>
        /// 已分配数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("已分配数量")]
        public int QtyAllocated { get; set; }

        /// <summary>
        /// 预移入数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("预移入数量")]
        public int QtyMoveIn { get; set; }

        /// <summary>
        /// 盘亏数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("盘亏数量")]
        public int QtySuspense { get; set; }

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
            this.InventoryId = CommonHelper.GetGuid;
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
            this.InventoryId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}