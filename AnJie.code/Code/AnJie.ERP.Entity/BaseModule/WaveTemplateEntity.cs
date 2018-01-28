using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 波次模板
    /// </summary>
    [Description("波次模板")]
    [PrimaryKey("TemplateId")]
    [TableName("WaveTemplate")]
    public class WaveTemplateEntity : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 波次模板主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("波次模板主键")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库主键")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 模板编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("模板编号")]
        public string Code { get; set; }

        /// <summary>
        /// 模板描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("模板描述")]
        public string Description { get; set; }

        /// <summary>
        /// 最小商品数
        /// </summary>
        /// <returns></returns>
        [DisplayName("最小商品数")]
        public int? MinProducts { get; set; }

        /// <summary>
        /// 最大商品数
        /// </summary>
        /// <returns></returns>
        [DisplayName("最大商品数")]
        public int? MaxProducts { get; set; }

        /// <summary>
        /// 最小订单数
        /// </summary>
        /// <returns></returns>
        [DisplayName("最小订单数")]
        public int? MinOrders { get; set; }

        /// <summary>
        /// 最大订单数
        /// </summary>
        /// <returns></returns>
        [DisplayName("最大订单数")]
        public int? MaxOrders { get; set; }

        /// <summary>
        /// 波次流程
        /// </summary>
        /// <returns></returns>
        [DisplayName("波次流程")]
        public string WaveFlowId { get; set; }

        /// <summary>
        /// 订单筛选器
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单筛选器")]
        public string OrderFilterId { get; set; }

        /// <summary>
        /// 是否自动执行
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否自动执行")]
        public int? IsAutoExecute { get; set; }

        /// <summary>
        /// 是否自动释放
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否自动释放")]
        public int? IsAutoRelease { get; set; }

        /// <summary>
        /// 拣货目的储位
        /// </summary>
        /// <returns></returns>
        [DisplayName("拣货目的储位")]
        public int? PickToLocation { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否启用")]
        public int? IsEnable { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("排序编号")]
        public int? SortCode { get; set; }

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

        #endregion

        #region 扩展操作
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
        #endregion
    }
}