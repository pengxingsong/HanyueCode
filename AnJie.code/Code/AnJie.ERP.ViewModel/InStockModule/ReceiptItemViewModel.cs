namespace AnJie.ERP.ViewModel.InStockModule
{
    /// <summary>
    /// �ջ�����ϸ��
    /// </summary>
    public class ReceiptItemViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string ReceiptId { get; set; }

        /// <summary>
        /// �ⲿ����
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        public string ProductId { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        public string Code { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        public string ProductName { get; set; }

        /// <summary>
        /// ����ͺ�
        /// </summary>
        /// <returns></returns>
        public string Specification { get; set; }

        /// <summary>
        /// ������λ
        /// </summary>
        /// <returns></returns>
        public string BaseUnit { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        public string BarCode { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int Qty { get; set; }

        /// <summary>
        /// ���ջ�����
        /// </summary>
        /// <returns></returns>
        public int ReceivedQty { get; set; }

    }
}