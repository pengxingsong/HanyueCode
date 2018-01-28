using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ����������ӱ�
    /// </summary>
    [Description("����������ӱ�")]
    [PrimaryKey("CodeSeriousId")]
    public class Base_CodeRuleSerious : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string CodeSeriousId { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����������")]
        public string CodeRuleId { get; set; }

        /// <summary>
        /// �û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�û�����")]
        public string UserId { get; set; }

        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ֵ")]
        public int NowValue { get; set; }

        /// <summary>
        /// �������ͣ�0-������ӣ�1-�û�ռ�����ӣ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������ͣ�0-������ӣ�1-�û�ռ�����ӣ�")]
        public string ValueType { get; set; }

        /// <summary>
        /// �ϴθ���ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϴθ���ʱ��")]
        public string LastUpdateDate { get; set; }

        /// <summary>
        /// ��Ч(1-δʹ�ã�0-��ʹ��)
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч(1-δʹ�ã�0-��ʹ��)")]
        public int Enabled { get; set; }

     
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CodeSeriousId = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.CodeRuleId = keyValue;
        }
        #endregion
    }
}