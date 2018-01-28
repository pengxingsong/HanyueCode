using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AnJie.ERP.Utilities
{
    public class MD5Util
    {
        static MD5 md5Hash = MD5.Create();
        static StringBuilder sb = new StringBuilder(100);

        public static string Md5(string md5)
        {
            byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(md5));
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            string s = sb.ToString();
            sb.Clear();
            return s;
        }

        public static byte[] Md5Bytes(string content)
        {
            byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(content));
            return bytes;
        }
    }
}
