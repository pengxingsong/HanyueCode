using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 交接单主表
    /// </summary>
    [Description("交接单主表")]
    [PrimaryKey("HandoverId")]
    [TableName("Handover")]
    public class HandoverEntity : BaseEntity
    {
        /// <summary>
        /// 交接单主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("交接单主键")]
        public string HandoverId { get; set; }

        /// <summary>
        /// 交接单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("交接单号")]
        public string HandoverNo { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流方式")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 是否已打印
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否已打印")]
        public int IsPrinted { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("打印时间")]
        public DateTime? PrintTime { get; set; }

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
            this.HandoverId = CommonHelper.GetGuid;
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
            this.HandoverId = keyValue;
        }
    }
}