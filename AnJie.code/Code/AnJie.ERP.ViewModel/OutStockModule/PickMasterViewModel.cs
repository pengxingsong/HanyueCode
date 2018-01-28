using System;
using System.ComponentModel;

namespace AnJie.ERP.ViewModel.OutStockModule
{
    /// <summary>
    /// Pick_Master
    /// </summary>
    public class PickMasterViewModel
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string PickId { get; set; }

        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <returns></returns>
        public string WarehouseId { get; set; }

        /// <summary>
        /// �ջ��ֿ�
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public string PickNo { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public string ZoneCode { get; set; }

        /// <summary>
        /// ���κ�
        /// </summary>
        /// <returns></returns>
        public string WaveNo { get; set; }

        /// <summary>
        /// ״̬
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