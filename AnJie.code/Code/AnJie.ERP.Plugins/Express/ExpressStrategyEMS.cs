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
    public class ExpressNumStrategyEMS: ExpressStrategyBase, IExpressStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExpressName
        {
            get { return "邮政EMS单号生成策略"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ExpressCode
        {
            get { return "EMS"; }
        }

        public int DisplayOrderId
        {
            get { return 1; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentExpressNum"></param>
        /// <returns></returns>
        public override string GetNextExpressNum(string currentExpressNum)
        {
            long num = Convert.ToInt64(currentExpressNum.Substring(2, 8));
            if (num < 99999999)
            {
                num = num + 1;
            }
            string str = num.ToString().PadLeft(8, '0');
            string str1 = string.Concat(currentExpressNum.Substring(0, 2), str, currentExpressNum.Substring(10, 1));
            str1 = string.Concat(currentExpressNum.Substring(0, 2), str, GetLastNum(str1), currentExpressNum.Substring(11, 2));
            return str1;
        }

        private string GetLastNum(string emsno)
        {
            List<char> list = emsno.ToList();
            char item = list[2];
            int num = int.Parse(item.ToString()) * 8;
            item = list[3];
            num = num + int.Parse(item.ToString()) * 6;
            item = list[4];
            num = num + int.Parse(item.ToString()) * 4;
            item = list[5];
            num = num + int.Parse(item.ToString()) * 2;
            item = list[6];
            num = num + int.Parse(item.ToString()) * 3;
            item = list[7];
            num = num + int.Parse(item.ToString()) * 5;
            item = list[8];
            num = num + int.Parse(item.ToString()) * 9;
            item = list[9];
            num = num + int.Parse(item.ToString()) * 7;
            num = 11 - num % 11;
            if (num == 10)
            {
                num = 0;
            }
            else if (num == 11)
            {
                num = 5;
            }
            return num.ToString();
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

            Regex regex = new Regex(@"^\d+$");
            bool isMatch = regex.IsMatch(expressNum);
            return isMatch;
        }
    }
}
