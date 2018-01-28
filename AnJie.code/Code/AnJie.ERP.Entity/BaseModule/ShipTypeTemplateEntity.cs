using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 面单模板表
    /// </summary>
    [Description("面单模板表")]
    [PrimaryKey("TemplateId")]
    [TableName("ShipType_Template")]
    public class ShipTypeTemplateEntity : BaseEntity
    {
        /// <summary>
        /// 模板主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("模板主键")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 所属物流方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("所属物流方式")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 面单名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("面单名称")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 面单文件
        /// </summary>
        /// <returns></returns>
        [DisplayName("面单文件")]
        public string BackgroundImage { get; set; }

        /// <summary>
        /// 是否是电子面单
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否是电子面单")]
        public int IsElectronicBill { get; set; }

        /// <summary>
        /// 宽度(mm)
        /// </summary>
        /// <returns></returns>
        [DisplayName("宽度(mm)")]
        public int Width { get; set; }

        /// <summary>
        /// 高度(mm)
        /// </summary>
        /// <returns></returns>
        [DisplayName("高度(mm)")]
        public int Height { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        /// <returns></returns>
        [DisplayName("模板内容")]
        public string TemplateContent { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效")]
        public int Enabled { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [DisplayName("排序码")]
        public int SortCode { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        [DisplayName("删除标记")]
        public int DeleteMark { get; set; }

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
            this.TemplateId = CommonHelper.GetGuid;
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
            this.TemplateId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
      
    }
}