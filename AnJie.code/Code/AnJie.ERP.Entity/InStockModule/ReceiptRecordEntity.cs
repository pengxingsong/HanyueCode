using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// Receipt_Record
    /// </summary>
    [Description("�ջ���¼")]
    [PrimaryKey("ItemId")]
    [TableName("Receipt_Record")]
    public class ReceiptRecordEntity : BaseEntity
    {
        /// <summary>
        /// ������ϸ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ϸ����")]
        public string ItemId { get; set; }

        /// <summary>
        /// �ջ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ�������")]
        public string ReceiptId { get; set; }

        /// <summary>
        /// �ջ�����ϸ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ�����ϸ����")]
        public string ReceiptItemId { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ����")]
        public string ProductId { get; set; }

        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string LocationId { get; set; }

        /// <summary>
        /// LocationCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("LocationCode")]
        public string LocationCode { get; set; }

        /// <summary>
        /// ReceivedQty
        /// </summary>
        /// <returns></returns>
        [DisplayName("ReceivedQty")]
        public int ReceivedQty { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <returns></returns>
        [DisplayName("Status")]
        public int Status { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����û�����")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����û�")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�ʱ��")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸��û�����")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸��û�")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ItemId = CommonHelper.GetGuid;
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
            this.ItemId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}