using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// Pick_Item
    /// </summary>
    [Description("配货明细")]
    [PrimaryKey("ItemId")]
    [TableName("Pick_Item")]
    public class PickItemEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string ItemId { get; set; }

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
        /// 拣货库区
        /// </summary>
        /// <returns></returns>
        [DisplayName("拣货库区")]
        public string ZoneCode { get; set; }

        /// <summary>
        /// 拣货目的储位
        /// </summary>
        /// <returns></returns>
        [DisplayName("拣货目的储位")]
        public string ToLocationCode { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 波次号
        /// </summary>
        /// <returns></returns>
        [DisplayName("波次号")]
        public string WaveNo { get; set; }

        /// <summary>
        /// 拣货单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("拣货单号")]
        public string PickNo { get; set; }

        /// <summary>
        /// 配货数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("配货数量")]
        public int Qty { get; set; }

        /// <summary>
        /// 已拣货数
        /// </summary>
        /// <returns></returns>
        [DisplayName("已拣货数")]
        public int QtyPicked { get; set; }

        /// <summary>
        /// 已分拣数
        /// </summary>
        /// <returns></returns>
        [DisplayName("已分拣数")]
        public int QtySorted { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public int Status { get; set; }

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
            this.ItemId = CommonHelper.GetGuid;
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
            this.ItemId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}