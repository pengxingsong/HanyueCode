using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Plugins.ExpressDocking
{
    /// <summary>
    /// 加密方案
    /// </summary>
    public class Encrypt
    {
        /// <summary>
        /// 快递鸟 数据签证加密
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="key">appkey</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string KdNiaoSingEncrypt(string content,string key,string charset)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                return Base64Encrypt(MD5Encrypt(content + key,charset), charset);
            }
            return Base64Encrypt(MD5Encrypt(content, charset), charset);
        }


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="charset">字符编码类型名称</param>
        /// <returns></returns>
        public static string MD5Encrypt(string str,string charset)
        {
            byte[] buffer=System.Text.Encoding.GetEncoding(charset).GetBytes(str);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider check = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] some = check.ComputeHash(buffer);
                string ret = "";
                foreach (byte item in some)
                {
                    if (item < 16)
                        ret += "0" + item.ToString("X");
                    else
                        ret += item.ToString("x");
                }
                return ret.ToLower();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Base64
        /// </summary>
        /// <param name="str"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string Base64Encrypt(string str,string charset)
        {
            try
            {
                return Convert.ToBase64String(System.Text.Encoding.GetEncoding(charset).GetBytes(str));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static  string HmacSha1Encrypt(string str,string charset)
        {
            Encoding encode = Encoding.GetEncoding(charset);
            byte[] byteData = encode.GetBytes(str);
            HMACSHA1 hmac = new HMACSHA1(byteData);
            //连接多个字符串加密流数据示例
            //byte[] byteData = encode.GetBytes(text);
            //byte[] byteKey = encode.GetBytes(key);
            //HMACSHA1 hmac = new HMACSHA1(byteKey);
            //CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
            //cs.Write(byteData, 0, byteData.Length);
            //cs.Close();
            return Convert.ToBase64String(hmac.Hash);
        }

    }
}
