using System;

namespace ZM.YiDASDK
{
    /// <summary>
    /// 时间戳扩展
    /// </summary>
    internal static class UnixUtils
    {
        /// <summary>
        /// Unix时间戳转DateTime
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime UnixToDateTime(this long timestamp)
        {
            DateTime time = timestamp.UnixToDateTimeOrNull() ?? DateTime.MinValue;
            return time;
        }

        public static DateTime? UnixToDateTimeOrNull(this long? timestamp)
        {
            return (timestamp ?? 0).UnixToDateTimeOrNull();
        }

        /// <summary>
        /// Unix时间戳转DateTime
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime? UnixToDateTimeOrNull(this long timestamp)
        {
            DateTime? time = null;
#pragma warning disable CS0618 // 'TimeZone' is obsolete: 'System.TimeZone has been deprecated.  Please investigate the use of System.TimeZoneInfo instead.'
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
#pragma warning restore CS0618 // 'TimeZone' is obsolete: 'System.TimeZone has been deprecated.  Please investigate the use of System.TimeZoneInfo instead.'
            int len = timestamp.ToString().Length;
            switch (len)
            {
                case 10://精确到秒
                    time = startTime.AddSeconds(timestamp);
                    break;

                case 13://精确到毫秒
                    time = startTime.AddMilliseconds(timestamp);
                    break;

                default:
                    break;
            }
            return time;
        }

        public static double UnixToTimestamp(this DateTime time, int type = 0)
        {
            double intResult = 0;
#pragma warning disable CS0618 // 'TimeZone' is obsolete: 'System.TimeZone has been deprecated.  Please investigate the use of System.TimeZoneInfo instead.'
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
#pragma warning restore CS0618 // 'TimeZone' is obsolete: 'System.TimeZone has been deprecated.  Please investigate the use of System.TimeZoneInfo instead.'
            switch (type)
            {
                case 0://精确到秒
                    intResult = (time - startTime).TotalMilliseconds;
                    break;

                case 1://精确到毫秒
                    intResult = (time - startTime).TotalSeconds;
                    break;

                default:
                    break;
            }
            return Math.Round(intResult, 0);
        }

        public static long UnixToTimestampLong(this DateTime time, int type = 0)
        {
            return (long)time.UnixToTimestamp(type);
        }
        public static long? UnixToTimestampLongOrNull(this DateTime? time, int type = 0)
        {
            if (time == null) { return null; }
            return (long)time?.UnixToTimestamp(type);
        }

        /// <summary>
        /// DateTime转时间戳
        /// </summary>
        /// <param name="time">DateTime时间</param>
        /// <param name="type">0为毫秒,1为秒</param>
        /// <returns></returns>
        public static string UnixToTimestampStr(this DateTime time, int type = 0)
        {
            return time.UnixToTimestamp(type).ToString();
        }
    }
}
