namespace HH.YiDASDK.Util;

public static class DateTimeExtensions
{
    private const long TICKS = 621355968000000000L;

    public static long ToUnixTimeSeconds(this DateTime date)
    {
        return (date.ToUniversalTime().Ticks - TICKS) / 10000000;
    }
    public static long ToUnixTimeMilliseconds(this DateTime date)
    {
        return (date.ToUniversalTime().Ticks - TICKS) / 10000;
    }
    public static DateTime FromUnixTimeMilliseconds(this long ts)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(ts).LocalDateTime;
    }
    public static DateTime FromUnixTimeMilliseconds(this string str)
    {
        _ = long.TryParse(str, out long ts);
        return FromUnixTimeMilliseconds(ts);
    }
    public static DateTime FromUnixTimeSeconds(this long ts)
    {
        return DateTimeOffset.FromUnixTimeSeconds(ts).LocalDateTime;
    }
    public static DateTime FromUnixTimeSeconds(this string str)
    {
        _ = long.TryParse(str, out long ts);
        return FromUnixTimeSeconds(ts);
    }
}