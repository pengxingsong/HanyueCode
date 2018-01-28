using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AnJie.ERP.Plugins.Express
{
    public class ExpressStrategyCommon : ExpressStrategyBase, IExpressStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExpressName
        {
            get { return "单号自增生成策略"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ExpressCode
        {
            get { return "Common"; }
        }

        public int DisplayOrderId
        {
            get { return 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressNum"></param>
        /// <returns></returns>
        public bool VerifyExpressNum(string expressNum)
        {
            if (string.IsNullOrWhiteSpace(expressNum))
            {
                return false;
            }

            if (expressNum.Length < 8 || expressNum.Length > 20)
            {
                return false;
            }

            Regex regex = new  Regex(@"^\d+$");
            bool isMatch = regex.IsMatch(expressNum);
            return isMatch;
        }
    }
}
