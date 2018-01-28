using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ����������ʵ��
    /// </summary>
    [Description("����������ʵ��")]
    [PrimaryKey("AttributeValueId")]
    public class Base_FormAttributeValue : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��������ʵ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������ʵ������")]
        public string AttributeValueId { get; set; }
        /// <summary>
        /// ģ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ������")]
        public string ModuleId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ObjectId { get; set; }
        /// <summary>
        /// ����Json
        /// </summary>
        /// <returns></returns>
        [DisplayName("����Json")]
        public string ObjectParameterJson { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.AttributeValueId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.AttributeValueId = keyValue;
                                            }
        #endregion
    }
}