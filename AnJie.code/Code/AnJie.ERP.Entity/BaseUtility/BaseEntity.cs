using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnJie.ERP.Entity
{
    /// <summary>
    /// 实体类基类
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseEntity()
        {

        }

        public virtual void Create()
        {
        }

        public virtual void Modify(string keyValue)
        {
        }
    }
}