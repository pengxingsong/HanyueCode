using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 编码规则种子表
    /// </summary>
    [Description("编码规则种子表")]
    [PrimaryKey("CodeSeriousId")]
    public class Base_CodeRuleSerious : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 种子主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("种子主键")]
        public string CodeSeriousId { get; set; }

        /// <summary>
        /// 编码规则主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("编码规则主键")]
        public string CodeRuleId { get; set; }

        /// <summary>
        /// 用户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("用户主键")]
        public string UserId { get; set; }

        /// <summary>
        /// 种子值
        /// </summary>
        /// <returns></returns>
        [DisplayName("种子值")]
        public int NowValue { get; set; }

        /// <summary>
        /// 种子类型（0-最大种子，1-用户占用种子）
        /// </summary>
        /// <returns></returns>
        [DisplayName("种子类型（0-最大种子，1-用户占用种子）")]
        public string ValueType { get; set; }

        /// <summary>
        /// 上次更新时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("上次更新时间")]
        public string LastUpdateDate { get; set; }

        /// <summary>
        /// 有效(1-未使用，0-已使用)
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效(1-未使用，0-已使用)")]
        public int Enabled { get; set; }

     
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CodeSeriousId = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.CodeRuleId = keyValue;
        }
        #endregion
    }
}