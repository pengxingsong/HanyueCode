using AnJie.ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AnJie.ERP.Plugins.ExpressDocking
{

    /// <summary>
    /// 快递鸟接口对接实现
    /// </summary>
    public class KdNiaoExpressDockingImpl : IKdNiaoExpressDocking, IExpressDocking
    {
        //请求地址
        private readonly string _requestUrl = "";

        public KdNiaoExpressDockingImpl(string requestUrl)
        {
            _requestUrl = requestUrl;
        }

        /// <summary>
        /// 获取面单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KdNiaoWaybillRespondData GetWayBill(KdNiaoRequestData request)
        {
            KdNiaoWaybillRespondData rd = new KdNiaoWaybillRespondData();
            string paraValidateMess = "";
            paraValidateMess=request.ValidateData();
            if (paraValidateMess != "")
            {
                rd.RequestDataValidateMess = paraValidateMess;
                rd.Success = false;
                rd.EBusinessID = request.EBusinessID;
                rd.Reason = "参数有误！";
                return rd;
            }
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("RequestData", HttpUtility.UrlEncode(request.RequestData, Encoding.UTF8));
            param.Add("EBusinessID", request.EBusinessID);
            param.Add("RequestType", request.RequestType);
            param.Add("DataSign", HttpUtility.UrlEncode(request.DataSign, Encoding.UTF8));
            param.Add("DataType", request.DataType);
            string result = request.SendPost(_requestUrl, param, "UTF-8");
            if (!string.IsNullOrWhiteSpace(result))
            {
                rd = JsonHelper.JsonToEntity<KdNiaoWaybillRespondData>(result);
            }
            else
            {
                rd.RequestDataValidateMess = paraValidateMess;
                rd.Success = false;
                rd.EBusinessID = request.EBusinessID;
                rd.Reason = "无响应结果！";
            }
            return rd;
        }
    }
}
