
namespace AnJie.ERP.ViewModel.LogisticsModule
{
    /// <summary>
    /// 配送商运费结算模板明细
    /// </summary>
    public class ShipVendorShipTemplateItemViewModel 
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string ItemId { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        /// <returns></returns>
        public string TemplateId { get; set; }

        /// <summary>
        /// 省主键
        /// </summary>
        /// <returns></returns>
        public string ProvinceId { get; set; }

        /// <summary>
        /// 省主键
        /// </summary>
        /// <returns></returns>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 市主键
        /// </summary>
        /// <returns></returns>
        public string CityId { get; set; }

        /// <summary>
        /// 市主键
        /// </summary>
        /// <returns></returns>
        public string CityName { get; set; }

        /// <summary>
        /// 县/区主键
        /// </summary>
        /// <returns></returns>
        public string CountyId { get; set; }

        /// <summary>
        /// 县/区主键
        /// </summary>
        /// <returns></returns>
        public string CountyName { get; set; }

        /// <summary>
        /// 起步价
        /// </summary>
        /// <returns></returns>
        public decimal? Price { get; set; }

        /// <summary>
        /// 加价
        /// </summary>
        /// <returns></returns>
        public decimal? AddPrice { get; set; }
    }
}