using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// Print_Plan
    /// </summary>
    [Description("Print_Plan")]
    [PrimaryKey("PlanId")]
    [TableName("Print_Plan")]
    public class PrintPlanEntity : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 批次方案主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("批次方案主键")]
        public string PlanId { get; set; }

        /// <summary>
        /// 方案名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("方案名称")]
        public string PlanName { get; set; }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <returns></returns>
        [DisplayName("执行SQL")]
        public string ExecuteSql { get; set; }

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

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.PlanId = CommonHelper.GetGuid;
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
            this.PlanId = keyValue;
                                            }
        #endregion
    }
}