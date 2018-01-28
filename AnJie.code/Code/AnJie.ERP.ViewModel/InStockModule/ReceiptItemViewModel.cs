namespace AnJie.ERP.ViewModel.InStockModule
{
    /// <summary>
    /// 收货单明细表
    /// </summary>
    public class ReceiptItemViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        public string ReceiptId { get; set; }

        /// <summary>
        /// 外部单号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        public string ProductId { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        /// <returns></returns>
        public string Code { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        /// <returns></returns>
        public string ProductName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        public string Specification { get; set; }

        /// <summary>
        /// 基本单位
        /// </summary>
        /// <returns></returns>
        public string BaseUnit { get; set; }

        /// <summary>
        /// 商品条码
        /// </summary>
        /// <returns></returns>
        public string BarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        public int Qty { get; set; }

        /// <summary>
        /// 已收货数量
        /// </summary>
        /// <returns></returns>
        public int ReceivedQty { get; set; }

    }
}