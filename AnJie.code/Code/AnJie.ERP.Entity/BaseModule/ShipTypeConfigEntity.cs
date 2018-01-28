using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 物流方式配置参数
    /// </summary>
    [Description("物流方式配置参数")]
    [PrimaryKey("ShipTypeId")]
    [TableName("ShipType_Config")]
    public class ShipTypeConfigEntity : BaseEntity
    {
        /// <summary>
        /// 配置参数主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("配置参数主键")]
        public string ConfigId { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流方式")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 参数名
        /// </summary>
        /// <returns></returns>
        [DisplayName("参数名")]
        public string ConfigField { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        /// <returns></returns>
        [DisplayName("参数值")]
        public string FieldValue { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [DisplayName("排序码")]
        public int? SortCode { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        [DisplayName("删除标记")]
        public int? DeleteMark { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        /// <returns></returns>
        [DisplayName("参数说明")]
        public string Memo { get; set; }

        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ConfigId = CommonHelper.GetGuid;
        }

        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ConfigId = keyValue;
        }
    }
}