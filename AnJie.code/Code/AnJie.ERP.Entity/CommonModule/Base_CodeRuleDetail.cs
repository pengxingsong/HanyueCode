using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ���������ϸ��
    /// </summary>
    [Description("���������ϸ��")]
    [PrimaryKey("CodeRuleDetailId")]
    public class Base_CodeRuleDetail : BaseEntity
    {
        /// <summary>
        /// ���������ϸ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������ϸ����")]
        public string CodeRuleDetailId { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����������")]
        public string CodeRuleId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string FullName { get; set; }

        /// <summary>
        /// �Ƿ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ���")]
        public string Consted { get; set; }

        /// <summary>
        /// �Ƿ��Զ���λ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ��Զ���λ")]
        public int? AutoReset { get; set; }

        /// <summary>
        /// �Ƿ񶨳�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ񶨳�")]
        public int? FixLength { get; set; }

        /// <summary>
        /// ��ʽ���ַ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ʽ���ַ���")]
        public string FormatStr { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public int? StepValue { get; set; }

        /// <summary>
        /// ��ʼֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ʼֵ")]
        public int? InitValue { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public int? FLength { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Remark { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string FType { get; set; }

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
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CodeRuleDetailId = CommonHelper.GetGuid;

        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.CodeRuleDetailId = keyValue;
        }
    }
}