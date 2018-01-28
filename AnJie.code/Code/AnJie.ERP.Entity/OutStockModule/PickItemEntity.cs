using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// Pick_Item
    /// </summary>
    [Description("�����ϸ")]
    [PrimaryKey("ItemId")]
    [TableName("Pick_Item")]
    public class PickItemEntity : BaseEntity
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string ItemId { get; set; }

        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֿ�����")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ����")]
        public string ProductId { get; set; }

        /// <summary>
        /// ��λ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ���")]
        public string LocationCode { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public string ZoneCode { get; set; }

        /// <summary>
        /// ���Ŀ�Ĵ�λ
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ŀ�Ĵ�λ")]
        public string ToLocationCode { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string OrderNo { get; set; }

        /// <summary>
        /// ���κ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���κ�")]
        public string WaveNo { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public string PickNo { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public int Qty { get; set; }

        /// <summary>
        /// �Ѽ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ѽ����")]
        public int QtyPicked { get; set; }

        /// <summary>
        /// �ѷּ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ѷּ���")]
        public int QtySorted { get; set; }

        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("״̬")]
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