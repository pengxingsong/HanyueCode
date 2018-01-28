using System;

namespace AnJie.ERP.ViewModel.OrderModule
{
    /// <summary>
    /// ��������
    /// </summary>
    public class SaleOrderViewModel
    {

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string OrderId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string OrderNo { get; set; }

        /// <summary>
        /// �ⲿ����
        /// </summary>
        /// <returns></returns>
        public string SourceOrderNo { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// �����ֿ�
        /// </summary>
        /// <returns></returns>
        public string WarehouseId { get; set; }

        /// <summary>
        /// �ջ��ֿ�
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        public string MerchantId { get; set; }

        /// <summary>
        /// �����̻�
        /// </summary>
        /// <returns></returns>
        public string MerchantName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string MerchantMallId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string MallName { get; set; }

        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <returns></returns>
        public string ShipTypeId { get; set; }

        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <returns></returns>
        public string ShipTypeName { get; set; }

        /// <summary>
        /// ������ʽ����
        /// </summary>
        public string ShipTypeCode { get; set; }

        /// <summary>
        /// ʡ����
        /// </summary>
        /// <returns></returns>
        public string ProvinceId { get; set; }

        /// <summary>
        /// ʡ
        /// </summary>
        /// <returns></returns>
        public string Province { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string CityId { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }

        /// <summary>
        /// ��/������
        /// </summary>
        /// <returns></returns>
        public string CountyId { get; set; }

        /// <summary>
        /// ��/��
        /// </summary>
        /// <returns></returns>
        public string County { get; set; }

        /// <summary>
        /// �ջ���ַ
        /// </summary>
        /// <returns></returns>
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// �ջ���
        /// </summary>
        /// <returns></returns>
        public string ReceiveContact { get; set; }

        /// <summary>
        /// �ջ��˵绰
        /// </summary>
        /// <returns></returns>
        public string ReceivePhone { get; set; }

        /// <summary>
        /// �ջ����ֻ�
        /// </summary>
        /// <returns></returns>
        public string ReceiveCellPhone { get; set; }

        /// <summary>
        /// �ջ����ʱ�
        /// </summary>
        /// <returns></returns>
        public string ReceiveZip { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string SellerNote { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public string BuyerNote { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string ExpressNum { get; set; }

        /// <summary>
        /// �Ƶ�������
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }

        /// <summary>
        /// �Ƶ���
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }

        /// <summary>
        /// �Ƶ�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// �޸�������
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// �޸���
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        public int Status { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }

        /// <summary>
        /// ��ӡ״̬
        /// </summary>
        /// <returns></returns>
        public int PrintStatus { get; set; }

        /// <summary>
        /// ��ӡʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? PrintTime { get; set; }

        /// <summary>
        /// �Ƿ�ҵ�
        /// </summary>
        /// <returns></returns>
        public bool IsSuspended { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductDetail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IsSuspendedShow
        {
            get
            {
                if (IsSuspended)
                {
                    return "<img src='../../Content/Images/checkokmark.gif'/>";
                }
                return "<img src='../../Content/Images/checknomark.gif'/>";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PrintStatusShow
        {
            get
            {
                switch (PrintStatus)
                {
                    case 0:
                        return "����ӡ";
                    case 1:
                        return "��ӡ��";
                    case 2:
                        return "�Ѵ�ӡ";
                    default:
                        return PrintStatus.ToString();
                }
            }
        }

        public string StatusShow
        {
            get
            {
                switch (Status)
                {
                    case 0:
                        return "��ʼ";
                    case -1:
                        return "ȱ��";
                    case -2:
                        return "������";
                    case 1:
                        return "<font color='blue'>�����</font>";
                    case 2:
                        return "<font color='blue'>������</font>";
                    case 4:
                        return "<font color='blue'>�����</font>";
                    case 6:
                        return "<font color='green'>������</font>";
                    case 8:
                        return "<font color='green'>�ѳ���</font>";
                    case 10:
                        return "�ѽ���";
                    default:
                        return Status.ToString();
                }
            }
        }

        public string ReceivePhoneShow
        {
            get
            {
                return string.Format("{0},{1}", ReceiveCellPhone, ReceiveCellPhone);
            }
        }
    }
}