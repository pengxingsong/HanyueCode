using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// �̻�������
    /// </summary>
    [Description("�̻�������")]
    [PrimaryKey("MerchantId")]
    [TableName("Merchant")]
    public class MerchantEntity : BaseEntity
    {
        /// <summary>
        /// �̻�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�̻�����")]
        public string MerchantId { get; set; }

        /// <summary>
        /// �̻�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�̻�����")]
        public string Code { get; set; }

        /// <summary>
        /// �̻�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�̻�����")]
        public string FullName { get; set; }

        /// <summary>
        /// �̻����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�̻����")]
        public string ShortName { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string Manager { get; set; }

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
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Fax { get; set; }

        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ʼ�")]
        public string Email { get; set; }

        /// <summary>
        /// ʡ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ʡ����")]
        public string ProvinceId { get; set; }

        /// <summary>
        /// ʡ
        /// </summary>
        /// <returns></returns>
        [DisplayName("ʡ")]
        public string Province { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string CityId { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��")]
        public string City { get; set; }

        /// <summary>
        /// ��/������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��/������")]
        public string CountyId { get; set; }

        /// <summary>
        /// ��/��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��/��")]
        public string County { get; set; }

        /// <summary>
        /// ��ϸ��ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ϸ��ַ")]
        public string Address { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string Bank { get; set; }


        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����˺�")]
        public string BankAccount { get; set; }

        /// <summary>
        /// �ʱ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʱ�")]
        public string PostalCode { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Remark { get; set; }

        /// <summary>
        /// ��Ч
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч")]
        public int? Enabled { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public int? SortCode { get; set; }

        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ɾ�����")]
        public int? DeleteMark { get; set; }

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
            this.MerchantId = CommonHelper.GetGuid;
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
            this.MerchantId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}