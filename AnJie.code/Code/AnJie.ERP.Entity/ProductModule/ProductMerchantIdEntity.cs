using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Entity
{
    public class ProductMerchantIdEntity : ProductEntity
    {

        /// <summary>
        /// 商户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("商户主键")]
        public string MerchantId { get; set; }
    }
}
