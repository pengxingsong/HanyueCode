using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// �̻��˷ѽ���ģ����ϸ
    /// </summary>
    [Description("�̻��˷ѽ���ģ����ϸ")]
    [PrimaryKey("ItemId")]
    [TableName("Merchant_ShipTemplateItem")]
    public class MerchantShipTemplateItemEntity : BaseEntity
    {
        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����̻�")]
        public string ItemId { get; set; }

        /// <summary>
        /// ģ��Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("ģ��Id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// ʡ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ʡ����")]
        public string ProvinceId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string CityId { get; set; }

        /// <summary>
        /// ��/������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��/������")]
        public string CountyId { get; set; }

        /// <summary>
        /// �𲽼�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�𲽼�")]
        public decimal? Price { get; set; }

        /// <summary>
        /// �Ӽ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ӽ�")]
        public decimal? AddPrice { get; set; }

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