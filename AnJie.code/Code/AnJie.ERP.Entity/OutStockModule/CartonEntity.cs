using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 包裹
    /// </summary>
    [Description("包裹")]
    [PrimaryKey("CartonId")]
    [TableName("Carton")]
    public class CartonEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string CartonId { get; set; }

        /// <summary>
        /// 包裹号
        /// </summary>
        /// <returns></returns>
        [DisplayName("包裹号")]
        public string CartonNum { get; set; }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库主键")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        [DisplayName("所属商户")]
        public string MerchantId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流方式")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 总包裹数
        /// </summary>
        /// <returns></returns>
        [DisplayName("总包裹数")]
        public int? TotalCount { get; set; }

        /// <summary>
        /// 当前包裹序号
        /// </summary>
        /// <returns></returns>
        [DisplayName("当前包裹序号")]
        public int? CurrentNum { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流单号")]
        public string ExpressNum { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        /// <returns></returns>
        [DisplayName("体积")]
        public decimal? Cube { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        /// <returns></returns>
        [DisplayName("重量")]
        public decimal? Weight { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public int? Status { get; set; }

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
            this.CartonId = CommonHelper.GetGuid;
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
            this.CartonId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}