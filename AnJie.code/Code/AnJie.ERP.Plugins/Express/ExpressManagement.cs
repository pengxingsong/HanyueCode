using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AnJie.ERP.Plugins.Express
{
    public class ExpressManagement
    {
        public static List<IExpressStrategy> GetAllExpress()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            List<IExpressStrategy> processors = new List<IExpressStrategy>();
            foreach (var t in types)
            {
                bool flag = typeof(IExpressStrategy).IsAssignableFrom(t);
                if (flag)
                {
                    IExpressStrategy obj = CreateObject(t.FullName) as IExpressStrategy;
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
