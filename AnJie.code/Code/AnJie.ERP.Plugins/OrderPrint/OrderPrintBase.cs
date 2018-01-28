using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnJie.ERP.ViewModel.OrderModule;

namespace AnJie.ERP.Plugins.OrderPrint
{
    public abstract class OrderPrintBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderView"></param>
        /// <param name="orderItemList"></param>
        /// <returns></returns>
        public virtual string GetTemplateContent(SaleOrderViewModel orderView,
            List<SaleOrderItemViewModel> orderItemList)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "<style>body{margin:0; padding:0;font-family: 'microsoft yahei',Helvetica;font-size:12px;color: #333;}.table-hd{ margin:0;line-height:30px; float:left; background: #f5f5f5;padding:0 10px;  margin-top:30px;}.table-hd strong{font-size:14px;font-weight:normal; float:left}.table-hd span{ font-weight:normal; font-size:12px;float:right}table{border: 1px solid #ddd;width:100%;border-collapse: collapse;border-spacing: 0; font-size:12px; float:left}table th,table td{border:1px solid #ddd;padding: 8px; text-align:center}table th{border-top:0;}</style>");
            stringBuilder.Append("<body>");
            stringBuilder.AppendFormat("<h3 class=\"table-hd\"><strong>{0}发货单</strong><span>订单号：{1}（{2}）</span></h3>",
                "", orderView.OrderNo, orderView.OrderDate.HasValue ? orderView.OrderDate.Value.ToString("") : "");
            stringBuilder.Append(
                "<table class=\"table table-bordered\"><thead><tr><th>商品名称</th><th>规格</th><th>数量</th><th>单价</th><th>总价</th></tr></thead><tbody>");
            foreach (SaleOrderItemViewModel orderItemInfo in orderItemList)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.AppendFormat("<td style=\"text-align:left\">{0}</td>", orderItemInfo.ProductId);
                stringBuilder.AppendFormat("<td>{0} {1} {2}</td>", orderItemInfo.ProductId, orderItemInfo.ProductId,
                    orderItemInfo.ProductId);
                stringBuilder.AppendFormat("<td>{0}</td>", orderItemInfo.Qty);
                stringBuilder.AppendFormat("<td>￥{0}</td>", "");
                stringBuilder.AppendFormat("<td>￥{0}</td>", "");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.AppendFormat(
                "<tr><td style=\"text-align:right\" colspan=\"6\"><span>商品总价：￥{0} &nbsp; 运费：￥{1}</span> &nbsp; <b>实付金额：￥{2}</b></td></tr>",
                "", "", "");
            stringBuilder.AppendLine("</tbody></table>");
            stringBuilder.Append("</body>");
            return stringBuilder.ToString();
        }
    }
}