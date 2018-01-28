using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ������ӡ����
    /// </summary>
    [Description("������ӡ����")]
    [PrimaryKey("BatchId")]
    [TableName("Print_Batch")]
    public class PrintBatchEntity : BaseEntity
    {
        /// <summary>
        /// ���δ�ӡ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���δ�ӡ����")]
        public string BatchId { get; set; }

        /// <summary>
        /// ��ӡʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ӡʱ��")]
        public DateTime? PrintTime { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public int? ItemCount { get; set; }

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

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.BatchId = CommonHelper.GetGuid;
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
            this.BatchId = keyValue;
                                            }
        #endregion
    }
}