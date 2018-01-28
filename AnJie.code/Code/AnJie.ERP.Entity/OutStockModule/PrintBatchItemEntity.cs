using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ������ӡ��ϸ
    /// </summary>
    [Description("������ӡ��ϸ")]
    [PrimaryKey("ItemId")]
    [TableName("Print_BatchItem")]
    public class PrintBatchItemEntity : BaseEntity
    {
        /// <summary>
        /// ���δ�ӡ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���δ�ӡ����")]
        public string ItemId { get; set; }

        /// <summary>
        /// ���δ�ӡ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���δ�ӡ����")]
        public string BatchId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string OrderNo { get; set; }

        #region ��չ����
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
        #endregion
    }
}