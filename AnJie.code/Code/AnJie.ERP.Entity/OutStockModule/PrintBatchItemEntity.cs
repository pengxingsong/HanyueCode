using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 批量打印明细
    /// </summary>
    [Description("批量打印明细")]
    [PrimaryKey("ItemId")]
    [TableName("Print_BatchItem")]
    public class PrintBatchItemEntity : BaseEntity
    {
        /// <summary>
        /// 批次打印主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("批次打印主键")]
        public string ItemId { get; set; }

        /// <summary>
        /// 批次打印主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("批次打印主键")]
        public string BatchId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
        public string OrderNo { get; set; }

        #region 扩展操作
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
        #endregion
    }
}