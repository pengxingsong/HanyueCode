using System;
using System.ComponentModel;

namespace AnJie.ERP.ViewModel.InStockModule
{
    /// <summary>
    /// �ջ�����ϸ��
    /// </summary>
    public class ReceiptRecordViewModel
    {

        /// <summary>
        /// 
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReceiptId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string ReceiptItemId { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        public string ProductId { get; set; }

        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        public string LocationId { get; set; }

        /// <summary>
        /// LocationCode
        /// </summary>
        /// <returns></returns>
        public string LocationCode { get; set; }

        /// <summary>
        /// ReceivedQty
        /// </summary>
        /// <returns></returns>
        public int ReceivedQty { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <returns></returns>
        public int Status { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }

        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }

    }
}