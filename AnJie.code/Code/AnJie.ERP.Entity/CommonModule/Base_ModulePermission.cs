using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ģ��Ȩ�ޱ�
    /// </summary>
    [Description("ģ��Ȩ�ޱ�")]
    [PrimaryKey("ModulePermissionId")]
    public class Base_ModulePermission : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ģ��Ȩ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ��Ȩ������")]
        public string ModulePermissionId { get; set; }
        /// <summary>
        /// �������:1-����2-��ɫ5-�û�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������:1-���� 2-��ɫ  5-�û�")]
        public string Category { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ObjectId { get; set; }
        /// <summary>
        /// ģ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ������")]
        public string ModuleId { get; set; }
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
            this.ModulePermissionId = CommonHelper.GetGuid;
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
            this.ModulePermissionId = keyValue;
                                            }
        #endregion
    }
}