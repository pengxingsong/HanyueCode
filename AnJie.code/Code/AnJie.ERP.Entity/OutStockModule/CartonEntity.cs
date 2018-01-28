using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ����
    /// </summary>
    [Description("����")]
    [PrimaryKey("CartonId")]
    [TableName("Carton")]
    public class CartonEntity : BaseEntity
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string CartonId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string CartonNum { get; set; }

        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֿ�����")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����̻�")]
        public string MerchantId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string OrderNo { get; set; }

        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ʽ")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// �ܰ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ܰ�����")]
        public int? TotalCount { get; set; }

        /// <summary>
        /// ��ǰ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ǰ�������")]
        public int? CurrentNum { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ExpressNum { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���")]
        public decimal? Cube { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public decimal? Weight { get; set; }

        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("״̬")]
        public int? Status { get; set; }

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

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�ʱ��")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸��û�����")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸��û�")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CartonId = CommonHelper.GetGuid;
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
            this.CartonId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}