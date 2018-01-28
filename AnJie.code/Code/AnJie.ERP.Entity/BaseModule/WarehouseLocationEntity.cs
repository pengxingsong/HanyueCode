using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 储位信息
    /// </summary>
    [Description("储位信息")]
    [PrimaryKey("LocationId")]
    [TableName("Warehouse_Location")]
    public class WarehouseLocationEntity : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 储位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("储位主键")]
        public string LocationId { get; set; }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("仓库主键")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 库区主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("库区主键")]
        public string ZoneId { get; set; }

        /// <summary>
        /// 储位编码
        /// </summary>
        /// <returns></returns>
        [DisplayName("储位编码")]
        public string Code { get; set; }

        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        [DisplayName("作业区域")]
        public string AreaCode { get; set; }

        /// <summary>
        /// 上架区域
        /// </summary>
        /// <returns></returns>
        [DisplayName("上架区域")]
        public string PutZone { get; set; }

        /// <summary>
        /// 配货区域
        /// </summary>
        /// <returns></returns>
        [DisplayName("配货区域")]
        public string AllocZone { get; set; }

        /// <summary>
        /// 盘点区域
        /// </summary>
        /// <returns></returns>
        [DisplayName("盘点区域")]
        public string CCZone { get; set; }

        /// <summary>
        /// 是否贵重品
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否贵重品")]
        public int? HighValue { get; set; }

        /// <summary>
        /// 最后盘点日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("最后盘点日期")]
        public DateTime? LastCCDate { get; set; }

        /// <summary>
        /// 巷道
        /// </summary>
        /// <returns></returns>
        [DisplayName("巷道")]
        public string Alsle { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        /// <returns></returns>
        [DisplayName("列")]
        public string Bay { get; set; }

        /// <summary>
        /// 层
        /// </summary>
        /// <returns></returns>
        [DisplayName("层")]
        public string Floor { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        /// <returns></returns>
        [DisplayName("长")]
        public decimal? Length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        /// <returns></returns>
        [DisplayName("宽")]
        public decimal? Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        /// <returns></returns>
        [DisplayName("高")]
        public decimal? Height { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        /// <returns></returns>
        [DisplayName("体积")]
        public decimal? Cube { get; set; }

        /// <summary>
        /// 承重
        /// </summary>
        /// <returns></returns>
        [DisplayName("承重")]
        public decimal? Weight { get; set; }

        /// <summary>
        /// 拣货顺序
        /// </summary>
        /// <returns></returns>
        [DisplayName("拣货顺序")]
        public string PickingSEQ { get; set; }

        /// <summary>
        /// 盘点顺序
        /// </summary>
        /// <returns></returns>
        [DisplayName("盘点顺序")]
        public string CycleCountSEQ { get; set; }

        /// <summary>
        /// 上架顺序
        /// </summary>
        /// <returns></returns>
        [DisplayName("上架顺序")]
        public string PutAwaySEQ { get; set; }

        /// <summary>
        /// 库位形式
        /// </summary>
        /// <returns></returns>
        [DisplayName("库位形式")]
        public string LocationClass { get; set; }

        /// <summary>
        /// 库位类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("库位类型")]
        public int? LocationType { get; set; }

        /// <summary>
        /// 流动等级(ABC)
        /// </summary>
        /// <returns></returns>
        [DisplayName("流动等级(ABC)")]
        public int? MovementType { get; set; }

        /// <summary>
        /// 混放商品
        /// </summary>
        /// <returns></returns>
        [DisplayName("混放商品")]
        public int? CommingleSKU { get; set; }

        /// <summary>
        /// 混放批号
        /// </summary>
        /// <returns></returns>
        [DisplayName("混放批号")]
        public int? CommingleLOT { get; set; }

        /// <summary>
        /// 是否释放LPN
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否释放LPN")]
        public int? IsLostLPN { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否启用")]
        public int IsEnable { get; set; }

        /// <summary>
        /// 是否容积校验
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否容积校验")]
        public bool? IsCheckCapacity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Comments { get; set; }

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
            this.LocationId = CommonHelper.GetGuid;
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
            this.LocationId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}