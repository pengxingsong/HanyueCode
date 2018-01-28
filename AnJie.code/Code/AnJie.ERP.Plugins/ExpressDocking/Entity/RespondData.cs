using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.ExpressDocking
{
    /// <summary>
    /// 响应数据基类
    /// </summary>
    public class RespondData
    {
        /// <summary>
        /// 请求参数验证信息(本系统定义)
        /// </summary>
        public string RequestDataValidateMess { get; set; }

        public RespondErrorData ErrorData{get;set;}

        public RespondSucceedData SucceedData { get; set; }
    }

    /// <summary>
    /// 响应异常数据基类
    /// </summary>
    public class RespondErrorData
    {

    }

    /// <summary>
    /// 相应成功数据基类
    /// </summary>
    public class RespondSucceedData
    {

    }

}
