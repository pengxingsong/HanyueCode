using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// �����û���ϵ��
    /// </summary>
    [Description("�����û���ϵ��")]
    [PrimaryKey("ObjectUserRelationId")]
    public class Base_ObjectUserRelation : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ

        /// <summary>
        /// �����û���ϵ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����û���ϵ����")]
        public string ObjectUserRelationId { get; set; }

        /// <summary>
        /// �������:1-����2-��ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������:1-����2-��ɫ")]
        public string Category { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ObjectId { get; set; }

        /// <summary>
        /// �û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�û�����")]
        public string UserId { get; set; }

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

        #endregion

        #region ��չ����

        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ObjectUserRelationId = CommonHelper.GetGuid;
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
            this.ObjectUserRelationId = keyValue;
        }

        #endregion
    }
}