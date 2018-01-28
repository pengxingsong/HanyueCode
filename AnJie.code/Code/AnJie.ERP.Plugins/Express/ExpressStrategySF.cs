using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.Express
{
    /// <summary>
    /// 顺丰快递单号
    /// </summary>
    public class ExpressNumStrategySf : ExpressStrategyBase, IExpressStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExpressName
        {
            get { return "顺丰单号生成策略"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ExpressCode
        {
            get { return "SF"; }
        }

        public int DisplayOrderId
        {
            get { return 2; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentExpressNum"></param>
        /// <returns></returns>
        public override string GetNextExpressNum(string currentExpressNum)
        {
            int i;
            long num;
            char item;
            int[] numArray = new int[12];
            int[] numArray1 = new int[12];
            List<char> list = currentExpressNum.ToList();
            string str = currentExpressNum.Substring(0, 11);
            string empty;
            if (currentExpressNum.Substring(0, 1) != "0")
            {
                num = Convert.ToInt64(str) + 1;
                empty = num.ToString();
            }
            else
            {
                num = Convert.ToInt64(str) + 1;
                empty = string.Concat("0", num.ToString());
            }
            for (i = 0; i < 12; i++)
            {
                item = list[i];
                numArray[i] = int.Parse(item.ToString());
            }
            for (i = 0; i < 11; i++)
            {
                item = empty[i];
                numArray1[i] = int.Parse(item.ToString());
            }
            if (!(numArray1[8] - numArray[8] != 1 || numArray[8] % 2 != 1))
            {
                if (numArray[11] - 8 < 0)
                {
                    numArray1[11] = numArray[11] - 8 + 10;
                }
                else
                {
                    numArray1[11] = numArray[11] - 8;
                }
            }
            else if (!(numArray1[8] - numArray[8] != 1 || numArray[8] % 2 != 0))
            {
                if (numArray[11] - 7 < 0)
                {
                    numArray1[11] = numArray[11] - 7 + 10;
                }
                else
                {
                    numArray1[11] = numArray[11] - 7;
                }
            }
            else if (!(numArray[9] != 3 && numArray[9] != 6 || numArray[10] != 9))
            {
                if (numArray[11] - 5 < 0)
                {
                    numArray1[11] = numArray[11] - 5 + 10;
                }
                else
                {
                    numArray1[11] = numArray[11] - 5;
                }
            }
            else if (numArray[10] == 9)
            {
                if (numArray[11] - 4 < 0)
                {
                    numArray1[11] = numArray[11] - 4 + 10;
                }
                else
                {
                    numArray1[11] = numArray[11] - 4;
                }
            }
            else if (numArray[11] - 1 < 0)
            {
                numArray1[11] = numArray[11] - 1 + 10;
            }
            else
            {
                numArray1[11] = numArray[11] - 1;
            }
            string str1 = string.Concat(empty, numArray1[11].ToString());
            return str1;
        }

        public bool VerifyExpressNum(string expressNum)
        {
            if (string.IsNullOrWhiteSpace(expressNum))
            {
                return false;
            }

            if (expressNum.Length != 12)
            {
                return false;
            }

            Regex regex = new Regex(@"^\d+$");
            bool isMatch = regex.IsMatch(expressNum);
            if (!isMatch)
            {
                return false;
            }

            string str = expressNum.Substring(0, 11);
            string currentExpressNum = GetNextExpressNum((long.Parse(str) - 1).ToString() + "0");
            if (currentExpressNum != expressNum)
            {
                return false;
            }
            return true;
        }

    }
}
