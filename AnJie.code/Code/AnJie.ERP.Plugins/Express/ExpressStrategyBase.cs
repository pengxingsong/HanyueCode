using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.Express
{
    public abstract class ExpressStrategyBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentExpressNum"></param>
        /// <returns></returns>
        public virtual string GetNextExpressNum(string currentExpressNum)
        {
            long num;
            if (!long.TryParse(currentExpressNum, out num))
            {
                throw new FormatException("快递单号格式不正确,正确的格式为数字");
            }
            return (num + 1).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentExpressNum"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual List<string> GetNextExpressNum(string currentExpressNum, int count)
        {
            List<string> expressNumList = new List<string>();
            string expressNum = currentExpressNum;
            for (int i = 0; i < count; i++)
            {
                expressNum = GetNextExpressNum(expressNum);
                expressNumList.Add(expressNum);
            }
            return expressNumList;
        }
    }
}
