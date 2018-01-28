using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.ExpressDocking
{
    /// <summary>
    /// 快递鸟电子面单-返回响应
    /// </summary>
    public class KdNiaoWaybillRespondData:RespondData
    {
        public string EBusinessID { get; set; }

        public bool Success { get; set; }

        public string ResultCode { get; set; }

        public string Reason { get; set; }

        public string UniquerRequestNumber { get; set; }

        public string Callback { get; set; }

        public string EstimatedDeliveryTime { get; set; }

        public string PrintTemplate { get; set; }

        public KdNiaoWaybillRespondData_Order Order { get; set; }

    }

    public class KdNiaoWaybillRespondData_Order
    {
        public string OrderCode { get; set; }

        public string ShipperCode { get; set; }

        public string LogisticCode { get; set; }

        public string MarkDestination { get; set; }

        public string OriginCode { get; set; }

        public string OriginName { get; set; }

        public string PackageCode { get; set; }
    }
}
