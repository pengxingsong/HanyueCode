using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// �ջ�������
    /// </summary>
    [Description("�ջ�������")]
    [PrimaryKey("ReceiptId")]
    [TableName("Receipt")]
    public class ReceiptEntity : BaseEntity
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ReceiptId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string ReceiptNo { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// �ջ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ�����")]
        public int? ReceiptType { get; set; }

        /// <summary>
        /// �ջ��ֿ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ��ֿ�")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����̻�")]
        public string MerchantId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string MerchantMallId { get; set; }

        /// <summary>
        /// ��Դ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Դ����")]
        public string SourceNo { get; set; }

        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("״̬")]
        public int Status { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("����")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// �Ƶ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƶ�������")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// �Ƶ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƶ���")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// �Ƶ�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƶ�ʱ��")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// �޸�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�������")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// �޸���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸���")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�ʱ��")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ReceiptId = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }

        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ReceiptId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}