using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.ViewModel.InventoryModule
{
    public class InventoryTransactionViewModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string TransactionId { get; set; }

        /// <summary>
        /// 交易种类（枚举：收货、拣货、...）
        /// </summary>
        /// <returns></returns>
        public int Type { get; set; }

        /// <summary>
        /// 交易种类
        /// </summary>
        public string TypeShow
        {
            get
            {
                switch (Type)
                {
                    case 1:
                        return "收货";
                    case 2:
                        return "出库";
                    case 3:
                        return "拣货";
                    default:
                        return Type.ToString();
                }
            }
        }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        public string WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        /// <returns></returns>
        public string ProductId { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        /// <returns></returns>
        public string ProductName { get; set; }

        /// <summary>
        /// 来源商户
        /// </summary>
        /// <returns></returns>
        public string MerchantFrom { get; set; }

        /// <summary>
        /// 来源商户名称
        /// </summary>
        /// <returns></returns>
        public string MerchantFromName { get; set; }

        /// <summary>
        /// 目的商户
        /// </summary>
        /// <returns></returns>
        public string MerchantTo { get; set; }

        /// <summary>
        /// 目的商户名称
        /// </summary>
        /// <returns></returns>
        public string MerchantToName { get; set; }

        /// <summary>
        /// 交易数量
        /// </summary>
        /// <returns></returns>
        public int Qty { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        /// <returns></returns>
        public string SourceNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
    }
}
