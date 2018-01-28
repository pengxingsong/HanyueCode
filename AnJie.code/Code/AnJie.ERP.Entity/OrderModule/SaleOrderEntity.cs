using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 订单主表
    /// </summary>
    [Description("订单主表")]
    [PrimaryKey("OrderId")]
    [TableName("SaleOrder")]
    public class SaleOrderEntity : BaseEntity
    {
        /// <summary>
        /// 订单主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单主键")]
        public string OrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 外部订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("外部订单号")]
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单日期")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 出货仓库
        /// </summary>
        /// <returns></returns>
        [DisplayName("出货仓库")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        [DisplayName("所属商户")]
        public string MerchantId { get; set; }

        /// <summary>
        /// 所属店铺
        /// </summary>
        /// <returns></returns>
        [DisplayName("所属店铺")]
        public string MerchantMallId { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流方式")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 省主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("省主键")]
        public string ProvinceId { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        /// <returns></returns>
        [DisplayName("省")]
        public string Province { get; set; }

        /// <summary>
        /// 市主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("市主键")]
        public string CityId { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        /// <returns></returns>
        [DisplayName("市")]
        public string City { get; set; }

        /// <summary>
        /// 县/区主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("县/区主键")]
        public string CountyId { get; set; }

        /// <summary>
        /// 县/区
        /// </summary>
        /// <returns></returns>
        [DisplayName("县/区")]
        public string County { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货地址")]
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货人")]
        public string ReceiveContact { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货人电话")]
        public string ReceivePhone { get; set; }

        /// <summary>
        /// 收货人手机
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货人手机")]
        public string ReceiveCellPhone { get; set; }

        /// <summary>
        /// 收货人邮编
        /// </summary>
        /// <returns></returns>
        [DisplayName("收货人邮编")]
        public string ReceiveZip { get; set; }

        /// <summary>
        /// 卖家留言
        /// </summary>
        /// <returns></returns>
        [DisplayName("卖家留言")]
        public string SellerNote { get; set; }

        /// <summary>
        /// 买家留言
        /// </summary>
        /// <returns></returns>
        [DisplayName("买家留言")]
        public string BuyerNote { get; set; }


        /// <summary>
        /// 物流单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("物流单号")]
        public string ExpressNum { get; set; }

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
        /// 修改人主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人主键")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 出库状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("波次状态")]
        public int OutStockStatus { get; set; }

        /// <summary>
        /// 打印状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("打印状态")]
        public int PrintStatus { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("打印时间")]
        public DateTime? PrintTime { get; set; }

        /// <summary>
        /// 打印人
        /// </summary>
        /// <returns></returns>
        [DisplayName("打印人主键")]
        public string PrintUserId { get; set; }

        /// <summary>
        /// 打印人
        /// </summary>
        /// <returns></returns>
        [DisplayName("打印人")]
        public string PrintUserName { get; set; }

        /// <summary>
        /// 是否挂单
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否挂单")]
        public bool IsSuspended { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("解锁时间")]
        public DateTime? UnLockTime { get; set; }

        /// <summary>
        /// 锁单人
        /// </summary>
        /// <returns></returns>
        [DisplayName("锁单人主键")]
        public string LockUserId { get; set; }

        /// <summary>
        /// 锁单人
        /// </summary>
        /// <returns></returns>
        [DisplayName("锁单人")]
        public string LockUserName { get; set; }

        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.OrderId = CommonHelper.GetGuid;
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
            this.OrderId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}