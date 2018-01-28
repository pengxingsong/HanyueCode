using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AnJie.ERP.Plugins.Express;
using AnJie.ERP.ViewModel.LogisticsModule;
using Newtonsoft.Json.Linq;

namespace AnJie.ERP.Service
{
    public class ExpressService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IExpressStrategy> GetAllExpress()
        {
            return ExpressManagement.GetAllExpress();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressCode"></param>
        /// <returns></returns>
        public IExpressStrategy GetExpress(string expressCode)
        {
            IEnumerable<IExpressStrategy> allExpress = GetAllExpress();
            return allExpress.FirstOrDefault((IExpressStrategy item) => item.ExpressCode == expressCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kuaidi100Code"></param>
        /// <param name="shipOrderNumber"></param>
        /// <returns></returns>
        public ExpressData GetExpressData(string kuaidi100Code, string shipOrderNumber)
        {
            string str = string.Format("http://www.kuaidi100.com/query?type={0}&postid={1}", kuaidi100Code, shipOrderNumber);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(str);
            httpWebRequest.Timeout = 8000;
            ExpressData expressDatum = new ExpressData();
            try
            {
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    expressDatum.Message = "网络错误";
                }
                else
                {
                    Stream responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                        StringBuilder stringBuilder = new StringBuilder(streamReader.ReadToEnd());
                        stringBuilder.Replace("&amp;", "").Replace("&nbsp;", "").Replace("&", "");
                        JObject jObjects = JObject.Parse(stringBuilder.ToString());
                        string str1 = "200";
                        if (jObjects["status"] == null || jObjects["status"].ToString() != str1)
                        {
                            expressDatum.Message = jObjects["message"].ToString();
                        }
                        else
                        {
                            JArray item = jObjects["data"] as JArray;
                            if (item == null)
                            {
                                throw new ApplicationException("查询源数据格式错误，没有找到data节点");
                            }
                            List<ExpressDataItem> expressDataItems = new List<ExpressDataItem>();
                            foreach (JToken jTokens in item)
                            {
                                if (jTokens["time"] == null)
                                {
                                    throw new ApplicationException("查询源数据格式错误，没有找到time节点");
                                }
                                if (jTokens["context"] == null)
                                {
                                    throw new ApplicationException("查询源数据格式错误，没有找到context节点");
                                }
                                ExpressDataItem expressDataItem = new ExpressDataItem()
                                {
                                    Time = DateTime.ParseExact(jTokens["time"].ToString(), "yyyy-MM-dd HH:mm:ss", null),
                                    Content = jTokens["context"].ToString()
                                };
                                expressDataItems.Add(expressDataItem);
                            }
                            expressDatum.Success = true;
                            expressDatum.ExpressDataItems = expressDataItems;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Log.Error(string.Format("快递查询错误:{{kuaidi100Code:{0},shipOrderNumber:{1}}}", kuaidi100Code, shipOrderNumber), exception);
                expressDatum.Message = "未知错误";
            }
            return expressDatum;
        }
    }
}
