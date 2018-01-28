using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ���ӵ�����
    /// </summary>
    [Description("���ӵ�����")]
    [PrimaryKey("HandoverId")]
    [TableName("Handover")]
    public class HandoverEntity : BaseEntity
    {
        /// <summary>
        /// ���ӵ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ӵ�����")]
        public string HandoverId { get; set; }

        /// <summary>
        /// ���ӵ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ӵ���")]
        public string HandoverNo { get; set; }

        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ʽ")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// �Ƿ��Ѵ�ӡ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ��Ѵ�ӡ")]
        public int IsPrinted { get; set; }

        /// <summary>
        /// ��ӡʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ӡʱ��")]
        public DateTime? PrintTime { get; set; }

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
            this.HandoverId = CommonHelper.GetGuid;
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
            this.HandoverId = keyValue;
        }
    }
}