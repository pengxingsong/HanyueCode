using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnJie.ERP.DataAccess.Attributes;
using AnJie.ERP.Utilities;
using System.ComponentModel;

namespace AnJie.ERP.Entity
{
    [Description("三方平台api设置对象实体")]
    [PrimaryKey("ID")]
    [TableName("Base_ForeignSysApi")]
    public class ForeignSysApiEntity:BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("主键")]
        public string ID { get; set; }

        /// <summary>
        /// 平台ID
        /// </summary>
        [DisplayName("平台ID")]
        public string PlatformID { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        [DisplayName("平台名称")]
        public string PlatformName { get; set; }

        /// <summary>
        /// Api方法名
        /// </summary>
        [DisplayName("Api方法名")]
        public string ApiMethod { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [DisplayName("标签")]
        public string Tag { get; set; }

        /// <summary>
        /// 是否收费(0否默认，1是)
        /// </summary>
        [DisplayName("是否收费(0否默认，1是)")]
        public bool? IsCollectFee { get; set; }

        /// <summary>
        /// 是否需要授权(0否，1是默认)
        /// </summary>
        [DisplayName("是否需要授权(0否，1是默认)")]
        public bool? IsAuth { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string Description { get; set; }


        #region 通用字段

        /// <summary>
        /// 是否删除(0否默认,1是)
        /// </summary>
        [DisplayName("是否删除(0否默认,1是)")]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 是否禁用(0否默认,1是)
        /// </summary>
        [DisplayName("是否禁用(0否默认,1是)")]
        public bool? IsDisabled { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建用户主键")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建用户")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改用户主键")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改用户")]
        public string ModifyUserName { get; set; }

        #endregion


        #region 操作

        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = CommonHelper.GetGuid;
            this.IsDeleted = false;
            this.IsDisabled =false;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }


        #endregion

    }
}
