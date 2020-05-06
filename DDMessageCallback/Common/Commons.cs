using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDMessageCallback.Common
{
    public static class Commons
    {
        /// <summary>
        /// ToInt32 扩展方法 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defV"></param>
        /// <returns></returns>
        public static int ToInt32(this object obj, int defV = 0)
        {
            int temp = 0;
            try
            {
                temp = Convert.ToInt32(obj);
            }
            catch
            {
                temp = defV;
            }
            return temp;
        }

        /// <summary>
        /// ToString 扩展方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defV"></param>
        /// <returns></returns>
        public static string ToStrings(this object obj, string defV = "")
        {
            string temp = "";
            try
            {
                temp = Convert.ToString(obj);
            }
            catch
            {
                temp = defV;
            }
            return temp;
        }
        /// <summary>
        /// 获取当前时间时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTime time)
        {
            TimeSpan ts = time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime StampToDateTime(this string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            string strZero = "0000000";
            if (timeStamp.Length == 13)
                strZero = "0000";
            long lTime = long.Parse(timeStamp + strZero);
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}