namespace AnJie.ERP.ViewModel.OrderModule
{
    /// <summary>
    /// 订单明细表
    /// </summary>
    public class SaleOrderItemViewModel
    {

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        public string ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        public string Qty { get; set; }

    }
}