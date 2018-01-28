using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.ExpressDocking
{

    public enum ExpressServiceType
    {
        /// <summary>
        /// 快递鸟
        /// </summary>
        KdNiao,
        /// <summary>
        /// 菜鸟
        /// </summary>
        CaiNiao
    }

    /// <summary>
    /// 对接平台工厂
    /// </summary>
    public class ExpressDockingFactory
    {
        public static IKdNiaoExpressDocking GetKdNiaoExpressDocking(string requestUrl)
        {
            return new KdNiaoExpressDockingImpl(requestUrl);
        }

    }
}
