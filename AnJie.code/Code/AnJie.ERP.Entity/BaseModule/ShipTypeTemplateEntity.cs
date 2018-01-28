using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// �浥ģ���
    /// </summary>
    [Description("�浥ģ���")]
    [PrimaryKey("TemplateId")]
    [TableName("ShipType_Template")]
    public class ShipTypeTemplateEntity : BaseEntity
    {
        /// <summary>
        /// ģ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ������")]
        public string TemplateId { get; set; }

        /// <summary>
        /// ����������ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������ʽ")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// �浥����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�浥����")]
        public string TemplateName { get; set; }

        /// <summary>
        /// �浥�ļ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�浥�ļ�")]
        public string BackgroundImage { get; set; }

        /// <summary>
        /// �Ƿ��ǵ����浥
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ��ǵ����浥")]
        public int IsElectronicBill { get; set; }

        /// <summary>
        /// ���(mm)
        /// </summary>
        /// <returns></returns>
        [DisplayName("���(mm)")]
        public int Width { get; set; }

        /// <summary>
        /// �߶�(mm)
        /// </summary>
        /// <returns></returns>
        [DisplayName("�߶�(mm)")]
        public int Height { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ������")]
        public string TemplateContent { get; set; }

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
        public int Enabled { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public int SortCode { get; set; }

        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ɾ�����")]
        public int DeleteMark { get; set; }

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