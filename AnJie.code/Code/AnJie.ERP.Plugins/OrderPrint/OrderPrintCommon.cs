using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AnJie.ERP.Plugins.OrderPrint
{
    public class OrderPrintCommon : OrderPrintBase, IOrderPrint
    {
        /// <summary>
        /// 
        /// </summary>
        public string TemplateName
        {
            get { return "普通发货单模板"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TemplateId
        {
            get { return "Common"; }
        }

        public int DisplayOrderId
        {
            get { return 0; }
        }
    }
}
