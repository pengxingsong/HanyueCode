using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnJie.ERP.ViewModel.OrderModule;

namespace AnJie.ERP.Plugins.OrderPrint
{
    public interface IOrderPrint
    {
        string TemplateName
        {
            get;
        }

        string TemplateId
        {
            get;
        }

        /// <summary>
        /// 顺序号
        /// </summary>
        int DisplayOrderId
        {
            get;
        }

        string GetTemplateContent(SaleOrderViewModel orderView, List<SaleOrderItemViewModel> orderItemList);
    }
}
