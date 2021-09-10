using System;
using System.Collections;

using UnityEngine;

namespace FrameWork
{
    /// <summary>
    /// 
    /// </summary>
    public static class TimeUtils
    {
        /// <summary>
        /// DateTime --> long
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>13位毫秒级</returns>
        public static long ToLong(this DateTime dt)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = dt.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
            return timeStamp;
        }

        /// <summary>
        ///DateTime --> int
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>10位秒级</returns>
        public static int ToInt(this DateTime dt)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = dt.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            return int.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 7));
        }

        /// <summary>
        /// int --> DateTime
        /// </summary>
        /// <param name="d"></param>
        /// <returns>时间</returns>
        public static DateTime ToDateTime(this int d)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse($"{d}0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime targetDt = dtStart.Add(toNow);
            return targetDt;
        }

        /// <summary>
        /// long --> DateTime
        /// </summary>
        /// <param name="d"></param>
        /// <returns>时间</returns>
        public static DateTime ToDateTime(this long d)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse($"{d}0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        /// <summary>
        /// 计算当前时间到零点的时间差--本地时间
        /// </summary>
        /// <param name="action">到零点时执行的方法</param>
        /// <returns></returns>
        private static string RefreshTaskTime(Action action)
        {
            DateTime TimeNow = DateTime.Now;
            DateTime TimeZero = TimeNow.AddDays(1).Date;
            TimeSpan ts = new TimeSpan(TimeNow.Ticks).Subtract(new TimeSpan(TimeZero.Ticks)).Duration();
            if (ts.Hours == 0 && ts.Minutes == 0 && ts.Seconds == 0)
                action();
            return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
        }

    }
}
