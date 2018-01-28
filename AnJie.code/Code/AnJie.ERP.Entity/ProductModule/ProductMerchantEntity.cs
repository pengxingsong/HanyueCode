using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ��Ʒ�̻���Ӧ��
    /// </summary>
    [Description("��Ʒ�̻���Ӧ��")]
    [PrimaryKey("RelationId")]
    [TableName("Product_Merchant")]
    public class ProductMerchantEntity : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// �̻��ֿ��ϵ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�̻��ֿ��ϵ����")]
        public string RelationId { get; set; }

        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֿ�����")]
        public string ProductId { get; set; }

        /// <summary>
        /// �̻�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�̻�����")]
        public string MerchantId { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����û�����")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����û�")]
        public string CreateUserName { get; set; }

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.RelationId = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RelationId = KeyValue;
                                            }
        #endregion
    }
}