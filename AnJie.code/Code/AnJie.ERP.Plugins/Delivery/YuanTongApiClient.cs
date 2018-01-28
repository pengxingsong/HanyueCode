using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AnJie.ERP.Utilities;

namespace AnJie.ERP.Express.Delivery
{
    public class YuanTongApiClient
    {
        /// <summary>
        /// 测试模式
        /// </summary>
        //const string ApiUrl = "http://58.32.246.71:8000/CommonOrderModeBPlusServlet.action";

        /// <summary>
        /// 正式模式
        /// </summary>
        const string ApiUrl = "http://service.yto56.net.cn/CommonOrderModeBPlusServlet.action ";

        /// <summary>
        /// ClientId/CustomerId
        /// </summary>
        private string ClientId = "K75707004";

        /// <summary>
        /// PartnerID
        /// </summary>
        private string PartnerId = "5NG6zr0H";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Post()
        {
            StringBuilder sbOrder = new StringBuilder();
            sbOrder.Append("<RequestOrder>");
            sbOrder.AppendFormat("  <clientID>{0}</clientID>", ClientId);//Y  商家代码（必须与customerId一致） 
            sbOrder.Append("  <logisticProviderID>YTO</logisticProviderID>");//Y 物流公司ID 必须为YTO
            sbOrder.AppendFormat("  <customerId>{0}</customerId>", ClientId);//Y 商家代码 (由商家设置， 必须与clientID一致)
            sbOrder.Append("  <txLogisticID>SO1020120</txLogisticID>");//Y 物流订单号
            sbOrder.Append("  <tradeNo></tradeNo>");//N 业务交易号（可选）
            sbOrder.Append("  <totalServiceFee>0.0</totalServiceFee>");//N 保值金额（暂时没有使用，默认为0.0）
            sbOrder.Append("  <codSplitFee>0.0</codSplitFee>");//N 物流公司分润[COD] （暂时没有使用，默认为0.0）
            sbOrder.Append("  <orderType>1</orderType>");//Y 订单类型(0-COD,1-普通订单,2-便携式订单3-退货单)
            sbOrder.Append("  <serviceType>0</serviceType>");//Y 服务类型(1-上门揽收, 2-次日达 4-次晨达 8-当日达,0-自己联系)。默认为0
            sbOrder.Append("  <flag>0</flag>");//N 订单flag标识，默认为 0，暂无意义
            sbOrder.Append("  <sender>");
            sbOrder.Append("    <name>张三</name>");
            sbOrder.Append("    <postCode></postCode>");//N 用户邮编 必须为正整数
            sbOrder.Append("    <phone></phone>");
            sbOrder.Append("    <mobile>13800138000</mobile>");
            sbOrder.Append("    <prov>上海</prov>");
            sbOrder.Append("    <city>上海,浦东新区</city>");
            sbOrder.Append("    <address>发件人地址发件人地址发件人地址</address>");
            sbOrder.Append("  </sender>");
            sbOrder.Append("  <receiver>");
            sbOrder.Append("    <name>李四</name>");//Y 用户姓名
            sbOrder.Append("    <postCode></postCode>");//N 用户邮编
            sbOrder.Append("    <phone></phone>");
            sbOrder.Append("    <mobile>13800138000</mobile>");//Y 用户移动电话， 手机和电话至少填一项
            sbOrder.Append("    <prov>上海</prov>");//Y 用户所在省
            sbOrder.Append("    <city>上海市,浦东新区</city>");//Y 用户所在市县（区），市区中间用英文“,”分隔；注意有些市下面没有区
            sbOrder.Append("    <address>收件人详细地址详细地址</address>");//Y 用户详细地址
            sbOrder.Append("  </receiver>");
            sbOrder.Append("  <sendStartTime>2016-07-13 00:00:00</sendStartTime>");//N 物流公司上门取货时间段，通过”yyyy-MM-dd HH:mm:ss”格式化，本文中所有时间格式相同。
            sbOrder.Append("  <sendEndTime>2016-07-14 00:00:00</sendEndTime>");//N 物流公司上门取货时间段，通过”yyyy-MM-dd HH:mm:ss”格式化，本文中所有时间格式相同。
            sbOrder.Append("  <goodsValue>1</goodsValue>");
            sbOrder.Append("  <items>");
            sbOrder.Append("    <item>");
            sbOrder.Append("      <itemName>商品名称</itemName>");//Y  商品名称 
            sbOrder.Append("      <number>1</number>");//Y 商品数量
            sbOrder.Append("      <itemValue>0</itemValue>");//N 商品单价（两位小数）
            sbOrder.Append("    </item>");
            sbOrder.Append("  </items>");
            sbOrder.Append("  <insuranceValue>0.0</insuranceValue>");//N 保值金额（暂时没有使用，默认为0.0）
            sbOrder.Append("  <special>1</special>");//N 商品类型（保留字段，暂时不用）
            sbOrder.Append("  <remark></remark>");//N 备注
            sbOrder.Append("</RequestOrder>");

            //&data_digest=pzknAMx0vCAVplwQamNzhg==&clientId=ClientId
            var md5 = MD5Util.Md5Bytes(sbOrder.ToString() + PartnerId);
            var dataDigest = System.Convert.ToBase64String(md5);

            var param = new Dictionary<string, string>
            {
                ["logistics_interface"] = sbOrder.ToString(),
                ["clientId"] = ClientId,
                ["data_digest"] = dataDigest
            };

            var ret = MSHttpRestful.DoWithRetry<string>(() => MSHttpRestful.PostUrlEncodeBodyReturnString(ApiUrl, param));
            if (string.IsNullOrWhiteSpace(ret))
            {
                throw new Exception("圆通系统没有返回任何数据");
            }

            try
            {
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("解析圆通数据失败:" + ex.Message, ex);
            }
        }
    }
}
