using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ������ʽ���ò���
    /// </summary>
    [Description("������ʽ���ò���")]
    [PrimaryKey("ShipTypeId")]
    [TableName("ShipType_Config")]
    public class ShipTypeConfigEntity : BaseEntity
    {
        /// <summary>
        /// ���ò�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ò�������")]
        public string ConfigId { get; set; }

        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ʽ")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string ConfigField { get; set; }

        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ֵ")]
        public string FieldValue { get; set; }

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
        /// ����˵��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����˵��")]
        public string Memo { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ConfigId = CommonHelper.GetGuid;
        }

        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ConfigId = keyValue;
        }
    }
}