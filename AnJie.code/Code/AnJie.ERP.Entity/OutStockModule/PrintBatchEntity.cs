using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 批量打印主表
    /// </summary>
    [Description("批量打印主表")]
    [PrimaryKey("BatchId")]
    [TableName("Print_Batch")]
    public class PrintBatchEntity : BaseEntity
    {
        /// <summary>
        /// 批次打印主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("批次打印主键")]
        public string BatchId { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("打印时间")]
        public DateTime? PrintTime { get; set; }

        /// <summary>
        /// 单据数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("单据数量")]
        public int? ItemCount { get; set; }

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

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.BatchId = CommonHelper.GetGuid;
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
            this.BatchId = keyValue;
                                            }
        #endregion
    }
}