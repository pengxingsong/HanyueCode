using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// ��λ��Ϣ
    /// </summary>
    [Description("��λ��Ϣ")]
    [PrimaryKey("LocationId")]
    [TableName("Warehouse_Location")]
    public class WarehouseLocationEntity : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string LocationId { get; set; }

        /// <summary>
        /// �ֿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֿ�����")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ZoneId { get; set; }

        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string Code { get; set; }

        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ҵ����")]
        public string AreaCode { get; set; }

        /// <summary>
        /// �ϼ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϼ�����")]
        public string PutZone { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public string AllocZone { get; set; }

        /// <summary>
        /// �̵�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�̵�����")]
        public string CCZone { get; set; }

        /// <summary>
        /// �Ƿ����Ʒ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ����Ʒ")]
        public int? HighValue { get; set; }

        /// <summary>
        /// ����̵�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����̵�����")]
        public DateTime? LastCCDate { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���")]
        public string Alsle { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��")]
        public string Bay { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��")]
        public string Floor { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��")]
        public decimal? Length { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��")]
        public decimal? Width { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��")]
        public decimal? Height { get; set; }

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
        /// ���˳��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���˳��")]
        public string PickingSEQ { get; set; }

        /// <summary>
        /// �̵�˳��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�̵�˳��")]
        public string CycleCountSEQ { get; set; }

        /// <summary>
        /// �ϼ�˳��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϼ�˳��")]
        public string PutAwaySEQ { get; set; }

        /// <summary>
        /// ��λ��ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ��ʽ")]
        public string LocationClass { get; set; }

        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public int? LocationType { get; set; }

        /// <summary>
        /// �����ȼ�(ABC)
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ȼ�(ABC)")]
        public int? MovementType { get; set; }

        /// <summary>
        /// �����Ʒ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����Ʒ")]
        public int? CommingleSKU { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public int? CommingleLOT { get; set; }

        /// <summary>
        /// �Ƿ��ͷ�LPN
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ��ͷ�LPN")]
        public int? IsLostLPN { get; set; }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ�����")]
        public int IsEnable { get; set; }

        /// <summary>
        /// �Ƿ��ݻ�У��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ��ݻ�У��")]
        public bool? IsCheckCapacity { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Comments { get; set; }

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

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.LocationId = CommonHelper.GetGuid;
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
            this.LocationId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}