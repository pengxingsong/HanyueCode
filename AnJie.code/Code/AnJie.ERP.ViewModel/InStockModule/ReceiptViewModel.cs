using System;

namespace AnJie.ERP.ViewModel.InStockModule
{
    /// <summary>
    /// �ջ���
    /// </summary>
    public class ReceiptViewModel
    {
        /// <summary>
        /// ��������
        /// </summary>
        public string ReceiptId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string ReceiptNo { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// �ջ�����
        /// </summary>
        public int? ReceiptType { get; set; }

        /// <summary>
        /// �ջ��ֿ�
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// �ջ��ֿ�
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        public string MerchantName { get; set; }

        /// <summary>
        /// ��Դ����
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// ���״̬��
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// �Ƶ�������
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// �Ƶ���
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// �Ƶ�ʱ��
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// �޸�������
        /// </summary>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// �޸���
        /// </summary>
        public string ModifyUserName { get; set; }

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLocked { get; set; }
    }
}