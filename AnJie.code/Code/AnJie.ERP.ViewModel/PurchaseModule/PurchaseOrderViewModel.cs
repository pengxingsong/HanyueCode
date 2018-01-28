using System;

namespace AnJie.ERP.ViewModel.PurchaseModule
{
    /// <summary>
    /// �ɹ�������
    /// </summary>
    public class PurchaseOrderViewModel
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string OrderId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string OrderNo { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// �ջ��ֿ�
        /// </summary>
        /// <returns></returns>
        public string WarehouseId { get; set; }

        /// <summary>
        /// �ջ��ֿ�
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        public string MerchantId { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        public string MerchantName { get; set; }

        /// <summary>
        /// ���״̬��
        /// </summary>
        /// <returns></returns>
        public int Status { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }

        /// <summary>
        /// �Ƶ�������
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }

        /// <summary>
        /// �Ƶ���
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }

        /// <summary>
        /// �Ƶ�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// �޸�������
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// �޸���
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
    }
}