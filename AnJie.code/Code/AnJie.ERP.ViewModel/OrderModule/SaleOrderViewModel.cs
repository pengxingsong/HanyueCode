using System;

namespace AnJie.ERP.ViewModel.OrderModule
{
    /// <summary>
    /// 订单主表
    /// </summary>
    public class SaleOrderViewModel
    {

        /// <summary>
        /// 订单主键
        /// </summary>
        /// <returns></returns>
        public string OrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        public string OrderNo { get; set; }

        /// <summary>
        /// 外部单号
        /// </summary>
        /// <returns></returns>
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        /// <returns></returns>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 出货仓库
        /// </summary>
        /// <returns></returns>
        public string WarehouseId { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        public string MerchantId { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// <returns></returns>
        public string MerchantName { get; set; }

        /// <summary>
        /// 所属店铺
        /// </summary>
        /// <returns></returns>
        public string MerchantMallId { get; set; }

        /// <summary>
        /// 所属店铺
        /// </summary>
        /// <returns></returns>
        public string MallName { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        /// <returns></returns>
        public string ShipTypeId { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        /// <returns></returns>
        public string ShipTypeName { get; set; }

        /// <summary>
        /// 物流方式代码
        /// </summary>
        public string ShipTypeCode { get; set; }

        /// <summary>
        /// 省主键
        /// </summary>
        /// <returns></returns>
        public string ProvinceId { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        /// <returns></returns>
        public string Province { get; set; }

        /// <summary>
        /// 市主键
        /// </summary>
        /// <returns></returns>
        public string CityId { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }

        /// <summary>
        /// 县/区主键
        /// </summary>
        /// <returns></returns>
        public string CountyId { get; set; }

        /// <summary>
        /// 县/区
        /// </summary>
        /// <returns></returns>
        public string County { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        /// <returns></returns>
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        /// <returns></returns>
        public string ReceiveContact { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        /// <returns></returns>
        public string ReceivePhone { get; set; }

        /// <summary>
        /// 收货人手机
        /// </summary>
        /// <returns></returns>
        public string ReceiveCellPhone { get; set; }

        /// <summary>
        /// 收货人邮编
        /// </summary>
        /// <returns></returns>
        public string ReceiveZip { get; set; }

        /// <summary>
        /// 卖家留言
        /// </summary>
        /// <returns></returns>
        public string SellerNote { get; set; }

        /// <summary>
        /// 买家留言
        /// </summary>
        /// <returns></returns>
        public string BuyerNote { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressNum { get; set; }

        /// <summary>
        /// 制单人主键
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 制单时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 修改人主键
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }

        /// <summary>
        /// 打印状态
        /// </summary>
        /// <returns></returns>
        public int PrintStatus { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>
        /// <returns></returns>
        public DateTime? PrintTime { get; set; }

        /// <summary>
        /// 是否挂单
        /// </summary>
        /// <returns></returns>
        public bool IsSuspended { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductDetail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IsSuspendedShow
        {
            get
            {
                if (IsSuspended)
                {
                    return "<img src='../../Content/Images/checkokmark.gif'/>";
                }
                return "<img src='../../Content/Images/checknomark.gif'/>";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PrintStatusShow
        {
            get
            {
                switch (PrintStatus)
                {
                    case 0:
                        return "待打印";
                    case 1:
                        return "打印中";
                    case 2:
                        return "已打印";
                    default:
                        return PrintStatus.ToString();
                }
            }
        }

        public string StatusShow
        {
            get
            {
                switch (Status)
                {
                    case 0:
                        return "初始";
                    case -1:
                        return "缺货";
                    case -2:
                        return "已作废";
                    case 1:
                        return "<font color='blue'>待审核</font>";
                    case 2:
                        return "<font color='blue'>待发货</font>";
                    case 4:
                        return "<font color='blue'>待拣货</font>";
                    case 6:
                        return "<font color='green'>待出库</font>";
                    case 8:
                        return "<font color='green'>已出库</font>";
                    case 10:
                        return "已交接";
                    default:
                        return Status.ToString();
                }
            }
        }

        public string ReceivePhoneShow
        {
            get
            {
                return string.Format("{0},{1}", ReceiveCellPhone, ReceiveCellPhone);
            }
        }
    }
}