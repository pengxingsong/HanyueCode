using System;
using System.ComponentModel;

namespace AnJie.ERP.ViewModel.BaseModule
{
    /// <summary>
    /// 面单模板表
    /// </summary>
    public class ShipTypeTemplateViewModel
    {
        /// <summary>
        /// 模板主键
        /// </summary>
        /// <returns></returns>
        public string TemplateId { get; set; }

        /// <summary>
        /// 所属物流方式
        /// </summary>
        /// <returns></returns>
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 所属物流方式
        /// </summary>
        public string ShipTypeName { get; set; }

        /// <summary>
        /// 面单名称
        /// </summary>
        /// <returns></returns>
        public string TemplateName { get; set; }

        /// <summary>
        /// 面单文件
        /// </summary>
        /// <returns></returns>
        public string BackgroundImage { get; set; }

        /// <summary>
        /// 是否是电子面单
        /// </summary>
        /// <returns></returns>
        public int IsElectronicBill { get; set; }

        /// <summary>
        /// 宽度(mm)
        /// </summary>
        /// <returns></returns>
        public int Width { get; set; }

        /// <summary>
        /// 高度(mm)
        /// </summary>
        /// <returns></returns>
        public int Height { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        /// <returns></returns>
        public string TemplateContent { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        public int Enabled { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        public int SortCode { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        public int DeleteMark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
    }
}