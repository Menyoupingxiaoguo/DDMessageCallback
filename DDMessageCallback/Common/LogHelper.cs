using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DDMessageCallback.Common
{
    public class LogHelper
    {
        static object locker = new object();

        /// <summary>  
        /// 写入所有日志  
        /// </summary>  
        /// <param name="logs">日志列表，每条日志占一行</param>  
        public static void WriteProgramLog(params string[] logs)
        {
            lock (locker)
            {
                string pro = System.AppDomain.CurrentDomain.BaseDirectory;
                string LogAddress = pro + @"\log";
                if (!Directory.Exists(LogAddress + "\\Log"))
                {
                    Directory.CreateDirectory(LogAddress + "\\Log");
                }
                LogAddress = string.Concat(LogAddress, "\\Log\\",
                 DateTime.Now.Year, '-', DateTime.Now.Month, '-',
                 DateTime.Now.Day, "_Log.log");
                StreamWriter sw = new StreamWriter(LogAddress, true);
                foreach (string log in logs)
                {
                    sw.WriteLine(log + "  at  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                sw.Close();
            }
        }
        /// <summary>  
        /// 写入所有日志到指定文件夹
        /// </summary>  
        /// <param name="log">日志</param>  
        public static void WriteProgramLogInFolder(string log, string Folder)
        {
            lock (locker)
            {
                string pro = System.AppDomain.CurrentDomain.BaseDirectory;
                string LogAddress = pro + @"\log";
                if (!Directory.Exists(LogAddress + "\\" + Folder))
                {
                    Directory.CreateDirectory(LogAddress + "\\" + Folder);
                }
                LogAddress = string.Concat(LogAddress, "\\" + Folder + "\\",
                DateTime.Now.Year, '-', DateTime.Now.Month, '-',
                DateTime.Now.Day, "_Log.log");
                StreamWriter sw = new StreamWriter(LogAddress, true);
                sw.WriteLine(log + "  at  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.Close();
            }
        }
    }
}