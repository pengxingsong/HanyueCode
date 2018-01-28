using System;
using System.ComponentModel;

namespace AnJie.ERP.ViewModel.OutStockModule
{
    /// <summary>
    /// Pick_Master
    /// </summary>
    public class PickMasterViewModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string PickId { get; set; }

        /// <summary>
        /// 仓库主键
        /// </summary>
        /// <returns></returns>
        public string WarehouseId { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        /// <returns></returns>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 拣货单号
        /// </summary>
        /// <returns></returns>
        public string PickNo { get; set; }

        /// <summary>
        /// 拣货库区
        /// </summary>
        /// <returns></returns>
        public string ZoneCode { get; set; }

        /// <summary>
        /// 波次号
        /// </summary>
        /// <returns></returns>
        public string WaveNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public int Status { get; set; }

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

        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
    }
}