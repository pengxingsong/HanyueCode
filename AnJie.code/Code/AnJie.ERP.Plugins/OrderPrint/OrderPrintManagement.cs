using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AnJie.ERP.Plugins.Express;

namespace AnJie.ERP.Plugins.OrderPrint
{
    public class OrderPrintManagement
    {
        public static List<IOrderPrint> GetAllOrderPrintTemplate()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            List<IOrderPrint> processors = new List<IOrderPrint>();
            foreach (var t in types)
            {
                bool flag = typeof(IOrderPrint).IsAssignableFrom(t);
                if (flag)
                {
                    IOrderPrint obj = CreateObject(t.FullName) as IOrderPrint;
                    if (obj != null)
                    {
                        processors.Add(obj);
                    }
                }
            }
            return processors.OrderBy(a=> a.DisplayOrderId).ToList();
        }

        /// <summary>
        /// 创建对象（当前程序集）
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <returns>创建的对象，失败返回 null</returns>
        private static object CreateObject(string typeName)
        {
            object obj = null;
            try
            {
                Type objType = Type.GetType(typeName, true);
                obj = Activator.CreateInstance(objType);
            }
            catch
            {
                // ignored
            }
            return obj;
        }

        /// <summary>
        /// 创建对象(外部程序集)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="typeName">类型名</param>
        /// <returns>创建的对象，失败返回 null</returns>
        public static object CreateObject(string path, string typeName)
        {
            object obj = null;
            try
            {
                obj = Assembly.Load(path).CreateInstance(typeName);
            }
            catch
            {
                // ignored
            }
            return obj;
        }
    }
}
