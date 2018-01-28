using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace AnJie.ERP.Utilities
{
    /// <summary>
    /// Log4Net日志 工厂
    /// </summary>
    public class LogFactory
    {
        static LogFactory()
        {
            FileInfo configFile = new FileInfo(SysHelper.GetMapPath("/XmlConfig/log4net.config"));
            if (configFile != null)
            {
                log4net.Config.XmlConfigurator.Configure(configFile);
            }
        }

        public static LogHelper GetLogger(Type type)
        {
            return new LogHelper(LogManager.GetLogger(type));
        }

        public static LogHelper GetLogger(string str)
        {
            return new LogHelper(LogManager.GetLogger(str));
        }
    }
}
