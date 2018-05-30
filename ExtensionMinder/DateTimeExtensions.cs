using System;
using System.Globalization;

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

        public static int CalculateAge(this DateTime dob)
        {
            return dob.CalculateAge(DateTime.Today);
        }

        public static int CalculateAge(this DateTime dob, DateTime atDate)
        {
            //Get difference in years
            var years = atDate.Date.Year - dob.Year;

            // subtract another year if we're before the
            // birth day in the current year
            if (atDate.Date.Month < dob.Month || (atDate.Date.Month == dob.Month && atDate.Date.Day < dob.Day))
            {
                --years;
            }

            return years;
        }

    /// <summary>
    /// Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
    /// </summary>
    /// <param name="source">Source (UTC format)</param>
    /// <param name="convertToUserTime">A value indicating whether we should convet DateTime instance to user local time (in case relative formatting is not applied)</param>
    /// <param name="defaultFormat">Default format string (in case relative formatting is not applied)</param>
    /// <returns>Formatted date and time string</returns>
    public static string RelativeFormat(this DateTime source, string defaultFormat)
    {
      string result = string.Empty;

      //TODO localize hard-coded strings
      var ts = new TimeSpan(DateTime.UtcNow.Ticks - source.Ticks);
      double delta = ts.TotalSeconds;

      if (delta > 0)
      {
        if (delta < 60) // 60 (seconds)
        {
          result = ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
        }
        else if (delta < 120) //2 (minutes) * 60 (seconds)
        {
          result = "a minute ago";
        }
        else if (delta < 2700) // 45 (minutes) * 60 (seconds)
        {
          result = ts.Minutes + " minutes ago";
        }
        else if (delta < 5400) // 90 (minutes) * 60 (seconds)
        {
          result = "an hour ago";
        }
        else if (delta < 86400) // 24 (hours) * 60 (minutes) * 60 (seconds)
        {
          int hours = ts.Hours;
          if (hours == 1)
            hours = 2;
          result = hours + " hours ago";
        }
        else if (delta < 172800) // 48 (hours) * 60 (minutes) * 60 (seconds)
        {
          result = "yesterday";
        }
        else if (delta < 2592000) // 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
        {
          result = ts.Days + " days ago";
        }
        else if (delta < 31104000) // 12 (months) * 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
        {
          int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
          result = months <= 1 ? "one month ago" : months + " months ago";
        }
        else
        {
          int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
          result = years <= 1 ? "one year ago" : years + " years ago";
        }
      }
      else
      {
        DateTime tmp1 = source;

        //default formatting
        if (!String.IsNullOrEmpty(defaultFormat))
        {
          result = tmp1.ToString(defaultFormat);
        }
        else
        {
          result = tmp1.ToString(CultureInfo.InvariantCulture);
        }
      }
      return result;
    }

  }
}
