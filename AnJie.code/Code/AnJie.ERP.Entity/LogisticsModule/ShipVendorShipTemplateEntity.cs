using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// �������˷ѽ���ģ��
    /// </summary>
    [Description("�������˷ѽ���ģ��")]
    [PrimaryKey("TemplateId")]
    [TableName("ShipVendor_ShipTemplate")]
    public class ShipVendorShipTemplateEntity : BaseEntity
    {
        /// <summary>
        /// ģ��Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ��Id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����̻�")]
        public string ShipVendorId { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ʽ")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ������")]
        public string TemplateName { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public decimal? Weight { get; set; }

        /// <summary>
        /// �Ӽ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ӽ�����")]
        public decimal? AddWeight { get; set; }

        /// <summary>
        /// Ĭ���𲽼�
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ĭ���𲽼�")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Ĭ�ϼӼ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ĭ�ϼӼ�")]
        public decimal? AddPrice { get; set; }

        /// <summary>
        /// ��Ч
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч")]
        public int? Enabled { get; set; }

        /// <summary>
        /// �Ƿ�Ĭ��ģ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ�Ĭ��ģ��")]
        public int? IsDefault { get; set; }

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
            this.TemplateId = CommonHelper.GetGuid;
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
            this.TemplateId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}