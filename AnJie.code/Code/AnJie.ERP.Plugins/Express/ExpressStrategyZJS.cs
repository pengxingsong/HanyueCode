using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.Express
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpressNumStrategyZjs : ExpressStrategyBase, IExpressStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExpressName
        {
            get { return "宅急送单号生成策略"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ExpressCode
        {
            get { return "ZJS"; }
        }

        public int DisplayOrderId
        {
            get { return 3; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentExpressNum"></param>
        /// <returns></returns>
        public override string GetNextExpressNum(string currentExpressNum)
        {
            long num = Convert.ToInt64(currentExpressNum) + 11;
            if (num % 10 > 6)
            {
                num = num - 7;
            }
            return num.ToString().PadLeft(currentExpressNum.Length, '0');
        }

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

            Regex regex = new Regex(@"^\d+$");
            bool isMatch = regex.IsMatch(expressNum);
            return isMatch;
        }
    }
}
