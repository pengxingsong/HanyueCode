using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// SaleOrder_ImportItem
    /// </summary>
    [Description("SaleOrder_ImportItem")]
    [PrimaryKey("ItemId")]
    [TableName("SaleOrder_ImportItem")]
    public class SaleOrderImportItemEntity : BaseEntity
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ItemId { get; set; }

        /// <summary>
        /// FileId
        /// </summary>
        /// <returns></returns>
        [DisplayName("FileId")]
        public string FileId { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ����")]
        public string ProductCode { get; set; }

        /// <summary>
        /// �ⲿ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ⲿ������")]
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// ��ϵ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ϵ��")]
        public string ReceiveContact { get; set; }

        /// <summary>
        /// �̶��绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("�̶��绰")]
        public string ReceivePhone { get; set; }

        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ϵ�绰")]
        public string ReceiveCellPhone { get; set; }

        /// <summary>
        /// ������˾
        /// </summary>
        /// <returns></returns>
        [DisplayName("������˾")]
        public string ShipTypeName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ExpressNum { get; set; }

        /// <summary>
        /// ���ʡ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ʡ��")]
        public string Province { get; set; }

        /// <summary>
        /// ��ҳ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ҳ���")]
        public string City { get; set; }

        /// <summary>
        /// ��ҵ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ҵ���")]
        public string County { get; set; }

        /// <summary>
        /// �ͻ���ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ͻ���ַ")]
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string SellerNote { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public string BuyerNote { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ����")]
        public int Qty { get; set; }


        /// <summary>
        /// �ջ����ʱ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ����ʱ�")]
        public string ReceiveZip { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Remark { get; set; }

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