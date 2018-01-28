using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 交接单明细
    /// </summary>
    [Description("交接单明细")]
    [PrimaryKey("ItemId")]
    [TableName("Handover_Item")]
    public class HandoverItemEntity : BaseEntity
    {
        /// <summary>
        /// 交接单明细主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("交接单明细主键")]
        public string ItemId { get; set; }

        /// <summary>
        /// 交接单主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("交接单主键")]
        public string HandoverId { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流单号")]
        public string ExpressNum { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 扫描时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("扫描时间")]
        public DateTime? ScanedTime { get; set; }

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