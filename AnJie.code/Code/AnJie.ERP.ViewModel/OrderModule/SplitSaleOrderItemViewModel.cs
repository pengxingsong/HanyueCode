namespace AnJie.ERP.ViewModel.OrderModule
{
    /// <summary>
    /// 拆分订单明细
    /// </summary>
    public class SplitSaleOrderItemViewModel
    {

        /// <summary>
        /// 订单明细ItemId
        /// </summary>
        /// <returns></returns>
        public string ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        public string Qty { get; set; }

        /// <summary>
        /// 拆分数量
        /// </summary>
        /// <returns></returns>
        public string SplitQty { get; set; }
    }
}
