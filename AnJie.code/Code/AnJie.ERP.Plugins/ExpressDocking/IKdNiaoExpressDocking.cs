using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.ExpressDocking
{
    /// <summary>
    /// 快递鸟平台对接接口
    /// </summary>
    public interface IKdNiaoExpressDocking:IExpressDocking
    {
        /// <summary>
        /// 获取电子面单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        KdNiaoWaybillRespondData GetWayBill(KdNiaoRequestData request);
    }
}
