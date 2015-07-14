using System;

namespace NetatmoBot.Extensions
{
    public static class DateExtension
    {
        public static long ToUnixTicks(this DateTime dateTime)
        {
            TimeSpan since = dateTime.Subtract(new DateTime(1970, 1, 1));
            return Convert.ToInt64(since.TotalSeconds);
        }

        public static DateTime? FromUnixTicks(this DateTime date, long? unixTicks)
        {
            if (unixTicks.HasValue)
            {
                return new DateTime(1970, 1, 1).AddSeconds(unixTicks.Value);
            }
            return null;
        }
    }
}