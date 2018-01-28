using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnJie.ERP.DataAccess.Attributes
{
    /// <summary>
    /// 主键字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TableNameAttribute : Attribute
    {
        public TableNameAttribute()
        {
        }

        public TableNameAttribute(string name)
        {
            _name = name;
        }

        private string _name;

        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }
}