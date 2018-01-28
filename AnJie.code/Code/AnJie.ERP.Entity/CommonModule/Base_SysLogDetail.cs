using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ϵͳ��־��ϸ
    /// </summary>
    [Description("ϵͳ��־��ϸ")]
    [PrimaryKey("SysLogDetailId")]
    public class Base_SysLogDetail : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ϵͳ��־��ϸ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ϵͳ��־��ϸ����")]
        public string SysLogDetailId { get; set; }
        /// <summary>
        /// ��־����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��־����")]
        public string SysLogId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string PropertyName { get; set; }
        /// <summary>
        /// �����ֶ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ֶ�")]
        public string PropertyField { get; set; }
        /// <summary>
        /// ������ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ֵ")]
        public string NewValue { get; set; }
        /// <summary>
        /// ���Ծ�ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ծ�ֵ")]
        public string OldValue { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CreateDate { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.SysLogDetailId = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
                                }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.SysLogDetailId = keyValue;
                                            }
        #endregion
    }
}