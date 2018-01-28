using System;
using System.ComponentModel;

namespace AnJie.ERP.ViewModel.OutStockModule
{
    /// <summary>
    /// Pick_Item
    /// </summary>
    public class PickItemViewModel
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string ItemId { get; set; }

        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <returns></returns>
        public string WarehouseId { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        public string ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// ��λ���
        /// </summary>
        /// <returns></returns>
        public string LocationCode { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public string ZoneCode { get; set; }

        /// <summary>
        /// ���Ŀ�Ĵ�λ
        /// </summary>
        /// <returns></returns>
        public string ToLocationCode { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string OrderNo { get; set; }

        /// <summary>
        /// ���κ�
        /// </summary>
        /// <returns></returns>
        public string WaveNo { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public string PickNo { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public int Qty { get; set; }

        /// <summary>
        /// �Ѽ����
        /// </summary>
        /// <returns></returns>
        public int QtyPicked { get; set; }

        /// <summary>
        /// �ѷּ���
        /// </summary>
        /// <returns></returns>
        public int QtySorted { get; set; }

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