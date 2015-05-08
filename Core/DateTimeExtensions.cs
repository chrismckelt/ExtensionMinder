using System;

namespace ExtensionMinder
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTimeToDateTimeUtc(this long secondsSinceEpoch)
        {
            return Epoch.AddSeconds(secondsSinceEpoch);
        }

        public static long DateTimeUtcToUnixTime(this DateTime dateTime)
        {
            return (long)((dateTime - Epoch).TotalSeconds);
        }

        public static bool IsBetween(this DateTime input, DateTime start, DateTime end)
        {
            return input.IsBetween(start, end, true);
        }

        public static bool IsBetween(this DateTime input, DateTime start, DateTime end, bool includeBoundaries)
        {
            return includeBoundaries
                ? (input >= start && input <= end)
                : (input > start && input < end);
        }

        public static string FormatFinancialYear(this DateTime date)
        {
            // Financial year for date 30/6/2012 = string "2011/2012", 01/07/2012 = "2012/2013"
            var year1 = date.Year - (date.Month <= 6 ? 1 : 0);
            var year2 = date.Year + (date.Month <= 6 ? 0 : 1);
            return String.Format("{0}/{1}", year1, year2);
        }

        public static DateTime GetFinancialYearStartDate(this DateTime date)
        {
            return new DateTime(date.Year - (date.Month <= 6 ? 1 : 0), 7, 01);
        }

        public static DateTime GetFinancialYearEndDate(this DateTime date)
        {
            return new DateTime(date.Year + (date.Month <= 6 ? 0 : 1), 6, 30);
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddSeconds(-1);
        }

        public static DateTime Min(DateTime t1, DateTime t2)
        {
            if (DateTime.Compare(t1, t2) > 0)
            {
                return t2;
            }
            return t1;
        }

        public static DateTime Max(DateTime t1, DateTime t2)
        {
            if (DateTime.Compare(t1, t2) < 0)
            {
                return t2;
            }
            return t1;
        }
    }
}
