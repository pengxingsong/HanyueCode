using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ���ӵ���ϸ
    /// </summary>
    [Description("���ӵ���ϸ")]
    [PrimaryKey("ItemId")]
    [TableName("Handover_Item")]
    public class HandoverItemEntity : BaseEntity
    {
        /// <summary>
        /// ���ӵ���ϸ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ӵ���ϸ����")]
        public string ItemId { get; set; }

        /// <summary>
        /// ���ӵ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ӵ�����")]
        public string HandoverId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ExpressNum { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string OrderNo { get; set; }

        /// <summary>
        /// ɨ��ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ɨ��ʱ��")]
        public DateTime? ScanedTime { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ItemId = CommonHelper.GetGuid;
        }

        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ItemId = keyValue;
        }
    }
}