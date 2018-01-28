using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.Express
{
    public interface IExpressStrategy
    {
        string ExpressName
        {
            get;
        }

        string ExpressCode
        {
            get;
        }

        /// <summary>
        /// 顺序号
        /// </summary>
        int DisplayOrderId
        {
            get;
        }

        /// <summary>
        /// 验证快递单号
        /// </summary>
        /// <param name="expressNum"></param>
        /// <returns></returns>
        bool VerifyExpressNum(string expressNum);

        string GetNextExpressNum(string currentExpressNum);

        List<string> GetNextExpressNum(string currentExpressNum, int count);
    }
}
