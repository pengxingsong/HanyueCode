using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ����ģ��
    /// </summary>
    [Description("����ģ��")]
    [PrimaryKey("TemplateId")]
    [TableName("WaveTemplate")]
    public class WaveTemplateEntity : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����ģ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ģ������")]
        public string TemplateId { get; set; }

        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֿ�����")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// ģ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ����")]
        public string Code { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ������")]
        public string Description { get; set; }

        /// <summary>
        /// ��С��Ʒ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��С��Ʒ��")]
        public int? MinProducts { get; set; }

        /// <summary>
        /// �����Ʒ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����Ʒ��")]
        public int? MaxProducts { get; set; }

        /// <summary>
        /// ��С������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��С������")]
        public int? MinOrders { get; set; }

        /// <summary>
        /// ��󶩵���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��󶩵���")]
        public int? MaxOrders { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WaveFlowId { get; set; }

        /// <summary>
        /// ����ɸѡ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ɸѡ��")]
        public string OrderFilterId { get; set; }

        /// <summary>
        /// �Ƿ��Զ�ִ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ��Զ�ִ��")]
        public int? IsAutoExecute { get; set; }

        /// <summary>
        /// �Ƿ��Զ��ͷ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ��Զ��ͷ�")]
        public int? IsAutoRelease { get; set; }

        /// <summary>
        /// ���Ŀ�Ĵ�λ
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ŀ�Ĵ�λ")]
        public int? PickToLocation { get; set; }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ�����")]
        public int? IsEnable { get; set; }

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

        #endregion

        #region ��չ����
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
        #endregion
    }
}