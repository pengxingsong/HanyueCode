using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnJie.ERP.Plugins.Express;
using AnJie.ERP.Plugins.OrderPrint;

namespace AnJie.ERP.Service
{
    public class PrintService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IOrderPrint> GetAllOrderPrintTemplate()
        {
            return OrderPrintManagement.GetAllOrderPrintTemplate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public IOrderPrint GetOrderPrintTemplate(string templateId)
        {
            IEnumerable<IOrderPrint> allExpress = GetAllOrderPrintTemplate();
            return allExpress.FirstOrDefault((IOrderPrint item) => item.TemplateId == templateId);
        }
    }
}
