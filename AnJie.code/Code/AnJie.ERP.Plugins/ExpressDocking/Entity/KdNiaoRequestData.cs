using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.ExpressDocking
{
    public class KdNiaoRequestData : RequestDataBase
    {
        /// <summary>
        /// R |
        /// 商户ID
        /// </summary>
        public virtual string EBusinessID { get; set; }

        public virtual string AppKey { get; set; }
        /// <summary>
        /// R |
        /// 请求指令类型：
        /// 电子面单:1007，即时查询:1002，单号识别:2002
        /// </summary>
        public virtual string RequestType { get; set; }

        /// <summary>
        /// R |
        /// 数据内容签名：把(请求内容(未编码)+AppKey)进行MD5加密，然后Base64编码，最后 进行URL(utf-8)编码。
        /// </summary>
        public virtual string DataSign { get; set; }

        /// <summary>
        /// O |
        /// 请求、返回数据类型：只支持JSON格式
        /// 2:Json
        /// </summary>
        public virtual string DataType { get; set; }

        /// <summary>
        /// R |
        /// 请求内容需进行URL(utf-8)编码。请求内容JSON格式，须和DataType一致。
        /// </summary>
        public virtual string RequestData { get; set; }


        public virtual KdNiaoWaybillRequestDataContent RequestDataContent { get; set; }

        public virtual string ValidateData()
        {
            StringBuilder vmes = new StringBuilder("");
            if (string.IsNullOrWhiteSpace(this.EBusinessID))
            {
                vmes.Append("商户ID必填！");
            }
            if (string.IsNullOrWhiteSpace(this.RequestData))
            {
                vmes.Append("请求数据必填必填！");
            }
            if (string.IsNullOrWhiteSpace(this.RequestType))
            {
                vmes.Append("请求指令类型必填！");
            }
            if (string.IsNullOrWhiteSpace(this.DataSign))
            {
                vmes.Append("数据内容签名必填！");
            }
            if (!string.IsNullOrWhiteSpace(vmes.ToString()))
            {
                return vmes.ToString();
            }
            if (this.RequestDataContent == null)
            {
                vmes.Append("数据参数信息必填！");
            }
            else if (this.RequestDataContent.ValidateData() != "")
            {
                vmes.Append(this.RequestDataContent.ValidateData());
            }
            return vmes.ToString();
        }

    }

    /// <summary>
    /// 快递鸟-电子面单请求数据
    /// </summary>
    public class KdNiaoWaybillRequestDataContent : RequestDataContent
    {
        /// <summary>
        /// O |
        /// 用户自定义回调信息
        /// </summary>
        public virtual string CallBack { get; set; }

        /// <summary>
        /// O |
        /// 会员标识
        /// </summary>
        public virtual string MemberID { get; set; }

        /// <summary>
        /// O |
        /// 电子面单客户账号（与快递网点申请）
        /// </summary>
        public virtual string CustomerName { get; set; }

        /// <summary>
        /// O |
        /// 电子面单密码
        /// </summary>
        public virtual string CustomerPwd { get; set; }

        /// <summary>
        /// O |
        /// 收件网点标识
        /// </summary>
        public virtual string SendSite { get; set; }

        /// <summary>
        /// R |
        /// 快递公司编码
        /// </summary>
        public virtual string ShipperCode { get; set; }

        /// <summary>
        /// O |
        /// 快递单号
        /// </summary>
        public virtual string LogisticCode { get; set; }

        /// <summary>
        /// R |
        /// 订单编号
        /// </summary>
        public virtual string OrderCode { get; set; }

        /// <summary>
        /// O |
        /// 第三方订单号
        /// </summary>
        public virtual string ThrOrderCode { get; set; }

        /// <summary>
        /// C |
        /// 月结编码
        /// </summary>
        public virtual string MonthCode { get; set; }

        /// <summary>
        /// R |
        /// 邮费支付方式:1-现付，2-到付，3-月结，4-第三方支付
        /// </summary>
        public virtual string PayType { get; set; }

        /// <summary>
        /// R |
        /// 快递类型：1-标准快件
        /// </summary>
        public virtual string ExpType { get; set; }

        /// <summary>
        /// O |
        /// 是否通知快递员上门揽件：0-通知；1-不通知；不填则默认为0
        /// </summary>
        public virtual string IsNotice { get; set; }

        /// <summary>
        /// O |
        /// 寄件费（运费）
        /// </summary>
        public virtual string Cost { get; set; }

        /// <summary>
        /// 其他费用
        /// </summary>
        public virtual string OtherCost { get; set; }

        /// <summary>
        /// O | 
        /// 上门取货开始时间("yyyy-MM-dd HH:mm:ss")
        /// </summary>
        public virtual string StartDate { get; set; }

        /// <summary>
        /// O | 
        /// 上门取货结束时间("yyyy-MM-dd HH:mm:ss")
        /// </summary>
        public virtual string EndDate { get; set; }

        /// <summary>
        /// O |
        /// 物品总重量（kg）
        /// </summary>
        public virtual string Weight { get; set; }

        /// <summary>
        /// O |
        /// 件数/包裹数
        /// </summary>
        public virtual string Quantity { get; set; }

        /// <summary>
        /// O |
        /// 物品总体积（立方米）
        /// </summary>
        public virtual string Volume { get; set; }

        /// <summary>
        /// O |
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// O |
        /// 返回电子面单模板：0-不需要；1-需要
        /// </summary>
        public virtual string IsReturnPrintTemplate { get; set; }

        /// <summary>
        /// R | 
        /// 收货信息
        /// </summary>
        public virtual KdNiaoAddress Receiver { get; set; }

        /// <summary>
        /// R | 
        /// 发货信息
        /// </summary>
        public virtual KdNiaoAddress Sender { get; set; }

        /// <summary>
        /// R |
        /// 商品列表
        /// </summary>
        public virtual List<KdNiaoCommodity> Commodity { get; set; }

        /// <summary>
        /// O |
        /// 增值服务列表
        /// </summary>
        public virtual List<KdNiaoAddService> AddServices { get; set; }

        public virtual string ValidateData()
        {
            StringBuilder vmes = new StringBuilder("");
            if (string.IsNullOrWhiteSpace(this.ShipperCode))
            {
                vmes.Append("快递公司编码必填！");
            }
            if (string.IsNullOrWhiteSpace(this.OrderCode))
            {
                vmes.Append("订单编号必填！");
            }
            if (string.IsNullOrWhiteSpace(this.PayType))
            {
                vmes.Append("邮费支付方式必填！");
            }
            if (string.IsNullOrWhiteSpace(this.ExpType))
            {
                vmes.Append("快递类型必填！");
            }
            if (string.IsNullOrWhiteSpace(this.IsReturnPrintTemplate))
            {
                vmes.Append("是否返回电子面单模板必填！");
            }
            if (this.Receiver == null)
            {
                vmes.Append("收货信息必填！");
            }
            else
            {
                var mes = this.Receiver.ValidateData();
                if (!string.IsNullOrWhiteSpace(mes))
                    vmes.Append("收货信息验证：[ " + mes + "]！");
            }

            if (this.Sender == null)
            {
                vmes.Append("发货信息必填！");
            }
            else
            {
                var mes = this.Sender.ValidateData();
                if (!string.IsNullOrWhiteSpace(mes))
                    vmes.Append("发货信息验证：[ " + mes + "]！");
            }

            if (this.Commodity == null || this.Commodity.Count == 0)
            {
                vmes.Append("商品信息至少填一项！");
            }
            else
            {
                foreach (var item in this.Commodity)
                {
                    var mes = item.ValidateData();
                    if (!string.IsNullOrWhiteSpace(mes))
                    {
                        vmes.Append(mes);
                        break;
                    }
                }
            }
            return vmes.ToString();
        }


    }

    /// <summary>
    /// 快递鸟-商品
    /// </summary>
    public class KdNiaoCommodity
    {
        /// <summary>
        /// R | 
        /// 商品名称
        /// </summary>
        public virtual string GoodsName { get; set; }

        /// <summary>
        /// O | 
        /// 商品编码
        /// </summary>
        public virtual string GoodsCode { get; set; }

        /// <summary>
        /// O | 
        /// 商品描述
        /// </summary>
        public virtual string GoodsDesc { get; set; }

        /// <summary>
        /// O | 
        /// 商品价格
        /// </summary>
        public virtual string GoodsPrice { get; set; }

        /// <summary>
        /// O | 
        /// 商品数量
        /// </summary>
        public virtual string Goodsquantity { get; set; }

        /// <summary>
        /// O | 
        /// 商品体积(立方米)
        /// </summary>
        public virtual string GoodsVol { get; set; }

        /// <summary>
        /// O | 
        /// 商品重量(kg)
        /// </summary>
        public virtual string GoodsWeight { get; set; }

        public virtual string ValidateData()
        {
            StringBuilder vmes = new StringBuilder("");
            if (string.IsNullOrWhiteSpace(this.GoodsName))
            {
                vmes.Append("商品名称必填！");
            }
            return vmes.ToString();
        }

    }

    /// <summary>
    /// 快递鸟-地址
    /// </summary>
    public class KdNiaoAddress
    {
        /// <summary>
        /// R |
        /// 详细地址
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// R |
        /// 省（如广东省，不要缺少“省”）
        /// </summary>
        public virtual string ProvinceName { get; set; }

        /// <summary>
        /// R | 
        /// 市（如深圳市，不要缺少“市”）
        /// </summary>
        public virtual string CityName { get; set; }

        /// <summary>
        /// O |
        /// 区（如福田区，不要缺少“区”或“县”）
        /// </summary>
        public virtual string ExpAreaName { get; set; }

        /// <summary>
        /// O |
        /// 邮编 
        /// </summary>
        public virtual string PostCode { get; set; }

        /// <summary>
        /// R |
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// R | 
        /// 移动电话(Mobile与Tel至少填一项)
        /// </summary>
        public virtual string Mobile { get; set; }

        /// <summary>
        /// R | 
        /// 固定电话(Mobile与Tel至少填一项)
        /// </summary>
        public virtual string Tel { get; set; }

        /// <summary>
        /// O |
        /// 公司名称
        /// </summary>
        public virtual string Company { get; set; }

        public virtual string ValidateData()
        {
            StringBuilder vmes = new StringBuilder("");
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                vmes.Append("客户姓名必填！");
            }
            if (string.IsNullOrWhiteSpace(this.Tel) && string.IsNullOrWhiteSpace(this.Mobile))
            {
                vmes.Append("移动电话或固定电话必须填一个！");
            }
            if (string.IsNullOrWhiteSpace(this.ProvinceName))
            {
                vmes.Append("省必填！");
            }
            if (string.IsNullOrWhiteSpace(this.CityName))
            {
                vmes.Append("市必填！");
            }
            if (string.IsNullOrWhiteSpace(this.Address))
            {
                vmes.Append("详细地址必填！");
            }
            return vmes.ToString();
        }

    }

    /// <summary>
    /// 快的鸟-增值服务
    /// </summary>
    public class KdNiaoAddService
    {
        /// <summary>
        /// O |
        /// 增值服务名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// O |
        /// 增值服务值
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// O |
        /// 客户标识（选填）
        /// </summary>
        public virtual string CustomerID { get; set; }

        public virtual string ValidateData()
        {
            return "";
        }
    }



}
