using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// �ֿ���Ϣ
    /// </summary>
    [Description("�ֿ���Ϣ")]
    [PrimaryKey("WarehouseId")]
    [TableName("Warehouse")]
    public class WarehouseEntity : BaseEntity
    {
        /// <summary>
        /// �ֿ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֿ���")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// WarehouseCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("WarehouseCode")]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֿ�����")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// ��˾����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��˾����")]
        public string CompanyId { get; set; }

        /// <summary>
        /// �ֿ��ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֿ��ַ")]
        public string Address { get; set; }

        /// <summary>
        /// ��ϵ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ϵ��")]
        public string Contact { get; set; }

        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ϵ�绰")]
        public string Phone { get; set; }

        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("״̬")]
        public int? Status { get; set; }

        /// <summary>
        /// �ʱ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʱ�")]
        public string PostalCode { get; set; }

        /// <summary>
        /// �ջ���ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ���ַ")]
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// �ջ���ϵ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ���ϵ��")]
        public string ReceiveContact { get; set; }

        /// <summary>
        /// �ջ���ϵ�˵绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ���ϵ�˵绰")]
        public string ReceivePhone { get; set; }

        /// <summary>
        /// ʡ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ʡ����")]
        public string ProvinceId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string CityId { get; set; }

        /// <summary>
        /// ��/������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��/������")]
        public string CountyId { get; set; }

        /// <summary>
        /// Ĭ���ջ���λ
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ĭ���ջ���λ")]
        public string ReceiptLocationId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public int? SortCode { get; set; }

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
            this.WarehouseId = CommonHelper.GetGuid;
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
            this.WarehouseId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}