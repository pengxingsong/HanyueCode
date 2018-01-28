using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ��������
    /// </summary>
    [Description("��������")]
    [PrimaryKey("OrderId")]
    [TableName("SaleOrder")]
    public class SaleOrderEntity : BaseEntity
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string OrderId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string OrderNo { get; set; }

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
        /// �����ֿ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ֿ�")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����̻�")]
        public string MerchantId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string MerchantMallId { get; set; }

        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ʽ")]
        public string ShipTypeId { get; set; }

        /// <summary>
        /// ʡ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ʡ����")]
        public string ProvinceId { get; set; }

        /// <summary>
        /// ʡ
        /// </summary>
        /// <returns></returns>
        [DisplayName("ʡ")]
        public string Province { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string CityId { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��")]
        public string City { get; set; }

        /// <summary>
        /// ��/������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��/������")]
        public string CountyId { get; set; }

        /// <summary>
        /// ��/��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��/��")]
        public string County { get; set; }

        /// <summary>
        /// �ջ���ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ���ַ")]
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// �ջ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ���")]
        public string ReceiveContact { get; set; }

        /// <summary>
        /// �ջ��˵绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ��˵绰")]
        public string ReceivePhone { get; set; }

        /// <summary>
        /// �ջ����ֻ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ����ֻ�")]
        public string ReceiveCellPhone { get; set; }

        /// <summary>
        /// �ջ����ʱ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ջ����ʱ�")]
        public string ReceiveZip { get; set; }

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
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ExpressNum { get; set; }

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
        /// �޸�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�������")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// �޸���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸���")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�ʱ��")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("״̬")]
        public int Status { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("����״̬")]
        public int OutStockStatus { get; set; }

        /// <summary>
        /// ��ӡ״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ӡ״̬")]
        public int PrintStatus { get; set; }

        /// <summary>
        /// ��ӡʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ӡʱ��")]
        public DateTime? PrintTime { get; set; }

        /// <summary>
        /// ��ӡ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ӡ������")]
        public string PrintUserId { get; set; }

        /// <summary>
        /// ��ӡ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ӡ��")]
        public string PrintUserName { get; set; }

        /// <summary>
        /// �Ƿ�ҵ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ�ҵ�")]
        public bool IsSuspended { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Remark { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? UnLockTime { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string LockUserId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string LockUserName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.OrderId = CommonHelper.GetGuid;
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
            this.OrderId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
    }
}