using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AnJie.ERP.Utilities
{
    public class MSHttpRestful
    {
        public static Regex REG_URL_ENCODING = new Regex(@"%[a-f0-9]{2}");

        static MSHttpRestful()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback
                = ((sender, cert, chain, errors) => cert.Subject.Contains("chuchujie"));
        }

        public static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
            {
                return "";
            }

            String stringToEncode = HttpUtility.UrlEncode(str, e).Replace("+", "%20").Replace("*", "%2A").Replace("(", "%28").Replace(")", "%29");
            return REG_URL_ENCODING.Replace(stringToEncode, m => m.Value.ToUpperInvariant());
        }

        private static System.Net.Http.HttpClient SetupClient(IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var client = new System.Net.Http.HttpClient { Timeout = new TimeSpan(0, 0, 20) };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.ParseAdd(accept == null ? "*/*" : accept);
            client.DefaultRequestHeaders.Referrer = referrer == null ? null : new Uri(referrer);
            if (headers != null)
            {
                foreach (var pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
                }
            }
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:47.0) Gecko/20100101 Firefox/47.0");
            return client;
        }

        public static String PostStringBodyReturnString(string url, string body, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var data = PostStringBodyReturnBytes(url, body, encoding, headers, referrer, accept);
            if (encoding != null)
            {
                return encoding.GetString(data);
            }
            return (encoding ?? Encoding.UTF8).GetString(data);
        }

        public static byte[] PostStringBodyReturnBytes(string url, string body, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var client = SetupClient(headers, referrer, accept);
            var ret = client.PostAsync(url, new System.Net.Http.StringContent(body ?? "", encoding ?? Encoding.UTF8)).Result;
            var data = ret.Content.ReadAsByteArrayAsync().Result;
            if (ret.IsSuccessStatusCode == false)
            {
                throw new Exception("HTTP请求错误:" + ret.StatusCode);
            }
            return data;
        }

        public static String PostBytesBodyReturnString(string url, byte[] body, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var data = PostBytesBodyReturnBytes(url, body, encoding, headers, referrer, accept);
            if (encoding != null)
            {
                return encoding.GetString(data);
            }
            return (encoding ?? Encoding.UTF8).GetString(data);
        }

        public static byte[] PostBytesBodyReturnBytes(string url, byte[] body, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var client = SetupClient(headers, referrer, accept);

            var ret = client.PostAsync(url, new System.Net.Http.ByteArrayContent(body ?? new byte[0])).Result;

            if (ret.IsSuccessStatusCode == false)
            {
                throw new Exception("HTTP请求错误:" + ret.StatusCode);
            }
            var data = ret.Content.ReadAsByteArrayAsync().Result;
            return data;
        }

        public static String PostStreamBodyReturnString(string url, Stream s, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var data = PostStreamBodyReturnBytes(url, s, encoding, headers, referrer, accept);
            if (encoding != null)
            {
                return encoding.GetString(data);
            }
            return (encoding ?? Encoding.UTF8).GetString(data);
        }

        public static byte[] PostStreamBodyReturnBytes(string url, Stream s, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var client = SetupClient(headers, referrer, accept);

            var ret = client.PostAsync(url, new System.Net.Http.StreamContent(s)).Result;
            if (ret.IsSuccessStatusCode == false)
            {
                throw new Exception("HTTP请求错误:" + ret.StatusCode);
            }
            var data = ret.Content.ReadAsByteArrayAsync().Result;
            return data;
        }

        public static string PostUrlEncodeBodyReturnString(string url, IDictionary<string, string> values, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var data = PostUrlEncodeBodyReturnBytes(url, values, encoding, headers, referrer, accept);
            if (encoding != null)
            {
                return encoding.GetString(data);
            }
            return (encoding ?? Encoding.UTF8).GetString(data);
        }

        public static byte[] PostUrlEncodeBodyReturnBytes(string url, IDictionary<string, string> values, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var client = SetupClient(headers, referrer, accept);
            var content = new System.Net.Http.FormUrlEncodedContent(values);
            content.Headers.ContentType.CharSet = (encoding ?? Encoding.UTF8).BodyName;
            content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
            var ret = client.PostAsync(url, content).Result;
            if (ret.IsSuccessStatusCode == false)
            {
                throw new Exception("HTTP请求错误:" + ret.StatusCode);
            }
            var data = ret.Content.ReadAsByteArrayAsync().Result;
            return data;
        }

        public static string PostMultipartFormDataBodyReturnString(string url, IDictionary<string, object> values, IList<string> fileNames, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var data = PostMultipartFormDataBodyReturnBytes(url, values, fileNames, encoding, headers, referrer, accept);
            return (encoding ?? Encoding.UTF8).GetString(data);
        }

        public static byte[] PostMultipartFormDataBodyReturnBytes(string url, IDictionary<string, object> values, IList<string> fileNames, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var content = new System.Net.Http.MultipartFormDataContent();
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            int i = 0;
            foreach (var item in values)
            {
                if (item.Value == null)
                {
                    content.Add(new System.Net.Http.StringContent(""), item.Key);
                }
                else if (item.Value.GetType() == typeof(byte[]))
                {
                    var cc = new System.Net.Http.StreamContent(new MemoryStream((byte[])item.Value));
                    cc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    content.Add(cc, item.Key, fileNames[i]);
                    i++;
                }
                else if (item.Value.GetType() == typeof(DateTime))
                {
                    content.Add(new System.Net.Http.StringContent(((DateTime)item.Value).ToString("yyyy-MM-dd HH:mm:ss")), item.Key);
                }
                else if (item.Value is Enum)
                {
                    content.Add(new System.Net.Http.StringContent(((int)item.Value).ToString()), item.Key);
                }
                else if (item.Value is double)
                {
                    content.Add(new System.Net.Http.StringContent(((double)item.Value).ToString("F2")), item.Key);
                }
                else if (item.Value is float)
                {
                    content.Add(new System.Net.Http.StringContent(((float)item.Value).ToString("F2")), item.Key);
                }
                else
                {
                    content.Add(new System.Net.Http.StringContent(item.Value.ToString()), item.Key);
                }
            }
            var client = SetupClient(headers, referrer, accept);
            var ret = client.PostAsync(url, content).Result;
            if (ret.IsSuccessStatusCode == false)
            {
                throw new Exception("HTTP请求错误:" + ret.StatusCode);
            }
            var data = ret.Content.ReadAsByteArrayAsync().Result;
            return data;
        }

        public static byte[] GetUrlEncodeBodyReturnBytes(string url, IDictionary<string, string> values, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            if (values != null && values.Count > 0)
            {
                url += "?" + string.Join("&", values.Select(obj => obj.Key + "=" + UrlEncode(obj.Value, encoding ?? Encoding.UTF8)));
            }
            var client = SetupClient(headers, referrer, accept);
            var ret = client.GetAsync(url).Result;
            if (ret.IsSuccessStatusCode == false)
            {
                throw new Exception("HTTP请求错误:" + ret.StatusCode);
            }
            var data = ret.Content.ReadAsByteArrayAsync().Result;
            return data;
        }

        public static string GetUrlEncodeBodyReturnString(string url, IDictionary<string, string> values, Encoding encoding = null, IDictionary<string, string> headers = null, string referrer = null, string accept = null)
        {
            var data = GetUrlEncodeBodyReturnBytes(url, values, encoding, headers, referrer, accept);
            return (encoding ?? Encoding.UTF8).GetString(data);
        }

        public static T DoWithRetry<T>(Func<T> func, int retryCount = 3)
        {
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    if (i == retryCount - 1)
                    {
                        throw ex;
                    }
                    Thread.Sleep((i + 1) * 1500);
                }
            }
            throw new Exception("代码应该永远执行不到这里");
        }

        public static string DoPost(string serviceUrl, Dictionary<string, string> parameter)
        {
            HttpWebRequest req = (HttpWebRequest)(WebRequest.Create(serviceUrl));
            req.ServicePoint.Expect100Continue = false;
            req.Method = "POST";
            req.KeepAlive = true;
            req.UserAgent = "sszg";
            req.Timeout = 5000;
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            string requestPram = string.Join("&", parameter.Select(item => item.Key + "=" + UrlEncode(item.Value, Encoding.GetEncoding("utf-8"))));

            byte[] postData = Encoding.UTF8.GetBytes(requestPram);
            var stream = req.GetRequestStream();
            stream.Write(postData, 0, postData.Length);
            stream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(rsp.GetResponseStream(), Encoding.GetEncoding(rsp.CharacterSet));
                string content = reader.ReadToEnd();
                return content;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                rsp.Close();
            }
        }
    }
}
