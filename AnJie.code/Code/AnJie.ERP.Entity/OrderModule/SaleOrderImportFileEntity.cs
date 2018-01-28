using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// SaleOrder_ImportFile
    /// </summary>
    [Description("SaleOrder_ImportFile")]
    [PrimaryKey("FileId")]
    [TableName("SaleOrder_ImportFile")]
    public class SaleOrderImportFileEntity : BaseEntity
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string FileId { get; set; }

        /// <summary>
        /// �ջ��ֿ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ��ֿ�")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����̻�")]
        public string MerchantId { get; set; }

        /// <summary>
        /// MerchantMallId
        /// </summary>
        /// <returns></returns>
        [DisplayName("MerchantMallId")]
        public string MerchantMallId { get; set; }

        /// <summary>
        /// FileName
        /// </summary>
        /// <returns></returns>
        [DisplayName("FileName")]
        public string FileName { get; set; }

        /// <summary>
        /// �Ƶ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƶ�������")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// �Ƶ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƶ���")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// �Ƶ�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƶ�ʱ��")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.FileId = CommonHelper.GetGuid;
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
            this.FileId = keyValue;
        }
    }
}