
namespace AnJie.ERP.ViewModel.LogisticsModule
{
    /// <summary>
    /// �������˷ѽ���ģ����ϸ
    /// </summary>
    public class ShipVendorShipTemplateItemViewModel 
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public string ItemId { get; set; }

        /// <summary>
        /// ģ��Id
        /// </summary>
        /// <returns></returns>
        public string TemplateId { get; set; }

        /// <summary>
        /// ʡ����
        /// </summary>
        /// <returns></returns>
        public string ProvinceId { get; set; }

        /// <summary>
        /// ʡ����
        /// </summary>
        /// <returns></returns>
        public string ProvinceName { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string CityId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string CityName { get; set; }

        /// <summary>
        /// ��/������
        /// </summary>
        /// <returns></returns>
        public string CountyId { get; set; }

        /// <summary>
        /// ��/������
        /// </summary>
        /// <returns></returns>
        public string CountyName { get; set; }

        /// <summary>
        /// �𲽼�
        /// </summary>
        /// <returns></returns>
        public decimal? Price { get; set; }

        /// <summary>
        /// �Ӽ�
        /// </summary>
        /// <returns></returns>
        public decimal? AddPrice { get; set; }
    }
}