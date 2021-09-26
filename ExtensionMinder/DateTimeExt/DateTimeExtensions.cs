using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExtensionMinder.DateTimeExt
{
    public static class DateTimeExtensions
    {
        private static readonly System.DateTime Epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static System.DateTime FromUnixTimeToDateTimeUtc(this long secondsSinceEpoch)
        {
            return Epoch.AddSeconds(secondsSinceEpoch);
        }

        public static long DateTimeUtcToUnixTime(this System.DateTime dateTime)
        {
            return (long) (dateTime - Epoch).TotalSeconds;
        }

        public static bool IsBetween(this System.DateTime input, System.DateTime start, System.DateTime end)
        {
            return input.IsBetween(start, end, true);
        }

        public static bool IsBetween(this System.DateTime input, System.DateTime start, System.DateTime end, bool includeBoundaries)
        {
            return includeBoundaries
                ? input >= start && input <= end
                : input > start && input < end;
        }

        public static string FormatFinancialYear(this System.DateTime date)
        {
            // Financial year for date 30/6/2012 = string "2011/2012", 01/07/2012 = "2012/2013"
            var year1 = date.Year - (date.Month <= 6 ? 1 : 0);
            var year2 = date.Year + (date.Month <= 6 ? 0 : 1);
            return $"{year1}/{year2}";
        }

        public static System.DateTime GetFinancialYearStartDate(this System.DateTime date)
        {
            return new System.DateTime(date.Year - (date.Month <= 6 ? 1 : 0), 7, 01);
        }

        public static System.DateTime GetFinancialYearEndDate(this System.DateTime date)
        {
            return new System.DateTime(date.Year + (date.Month <= 6 ? 0 : 1), 6, 30);
        }

        public static System.DateTime EndOfDay(this System.DateTime date)
        {
            return date.Date.AddDays(1).AddSeconds(-1);
        }

        public static System.DateTime Min(System.DateTime t1, System.DateTime t2)
        {
            if (System.DateTime.Compare(t1, t2) > 0) return t2;
            return t1;
        }

        public static System.DateTime Max(System.DateTime t1, System.DateTime t2)
        {
            if (System.DateTime.Compare(t1, t2) < 0) return t2;
            return t1;
        }

        public static int CalculateAge(this System.DateTime dob)
        {
            return dob.CalculateAge(System.DateTime.Today);
        }

        public static int CalculateAge(this System.DateTime dob, System.DateTime atDate)
        {
            //Get difference in years
            var years = atDate.Date.Year - dob.Year;

            // subtract another year if we're before the
            // birth day in the current year
            if (atDate.Date.Month < dob.Month || atDate.Date.Month == dob.Month && atDate.Date.Day < dob.Day) --years;

            return years;
        }

        /// <summary>
        /// Adds the given number of business days to the <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The date to be changed.</param>
        /// <param name="days">Number of business days to be added.</param>
        /// <returns>A <see cref="DateTime"/> increased by a given number of business days.</returns>
        public static DateTime AddBusinessDays(this DateTime current, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                }
                while (current.DayOfWeek == DayOfWeek.Saturday ||
                       current.DayOfWeek == DayOfWeek.Sunday);
            }
            return current;
        }

        public static int GetDaysFromLastMonday(this DateTime dt)
        {
            var day = dt.DayOfWeek.ToString();
            switch (day)
            {
                case "Sunday": return -6;
                case "Saturday": return -5;
                case "Friday": return -4;
                case "Thursday": return -3;
                case "Wednesday": return -2;
                case "Tuesday": return -1;
                case "Monday": return -7;
            }

            return 0;
        }

        public static int WeekDaysInMonthCount(this DateTime dt)
        {
            var year = dt.Year;
            var month = dt.Month;
            int days = DateTime.DaysInMonth(year, month);
            List<DateTime> dates = new List<DateTime>();
            for (int i = 1; i <= days; i++)
            {
                dates.Add(new DateTime(year, month, i));
            }

            int weekDays = dates.Count(d => d.DayOfWeek > DayOfWeek.Sunday & d.DayOfWeek < DayOfWeek.Saturday);
            return weekDays;
        }

        public static IEnumerable<DateTime> WeekDaysInMonthList(this DateTime dt)
        {
            var year = dt.Year;
            var month = dt.Month;
            int days = DateTime.DaysInMonth(year, month);
            List<DateTime> dates = new List<DateTime>();
            for (int i = 1; i <= days; i++)
            {
                var dt2 = new DateTime(year, month, i);
                if (dt2.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday) yield return dt2;
            }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/8561782/how-to-group-dates-by-week
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime StartOfWeek(this DateTime? dt, DayOfWeek startOfWeek)
        {
            return StartOfWeek(dt.GetValueOrDefault(), startOfWeek);
        }

        /// <summary>
        ///     Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
        /// </summary>
        /// <param name="source">Source (UTC format)</param>
        /// <param name="defaultFormat">Default format string (in case relative formatting is not applied)</param>
        /// <returns>Formatted date and time string</returns>
        public static string RelativeFormat(this System.DateTime source, string defaultFormat)
        {
            string result;

            //TODO localize hard-coded strings
            var ts = new TimeSpan(System.DateTime.UtcNow.Ticks - source.Ticks);
            var delta = ts.TotalSeconds;

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
                    var hours = ts.Hours;
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
                    var months = Convert.ToInt32(System.Math.Floor((double) ts.Days / 30));
                    result = months <= 1 ? "one month ago" : months + " months ago";
                }
                else
                {
                    var years = Convert.ToInt32(System.Math.Floor((double) ts.Days / 365));
                    result = years <= 1 ? "one year ago" : years + " years ago";
                }
            }
            else
            {
                var tmp1 = source;

                //default formatting
                if (!string.IsNullOrEmpty(defaultFormat))
                    result = tmp1.ToString(defaultFormat);
                else
                    result = tmp1.ToString(CultureInfo.InvariantCulture);
            }

            return result;
        }

        public static System.DateTime? ToAustralianDate(this string date,
            DateTimeExtensions.DateTimeFormat from = DateTimeExtensions.DateTimeFormat.UkDate)
        {
            System.DateTime dt;
            if (date.TryParseDateTime(from,out dt)) return dt;

            return Convert.ToDateTime(date, DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     Amount of seconds elapsed between 1970-01-01 00:00:00 and the date-time.
        /// </summary>
        /// <param name="dateTime">date-time</param>
        /// <returns>seconds</returns>
        public static uint GetSecondsSinceUnixEpoch(this System.DateTime dateTime)
        {
            var t = dateTime - new System.DateTime(1970, 1, 1);
            var ss = (int) t.TotalSeconds;
            if (ss < 0)
                return 0;
            return (uint) ss;
        }

        public static System.DateTime AddWorkDays(this System.DateTime date, int workingDays)
        {
            int direction = workingDays < 0 ? -1 : 1;
            System.DateTime newDate = date;
            while (workingDays != 0)
            {
                newDate = newDate.AddDays(direction);
                if (newDate.DayOfWeek != DayOfWeek.Saturday &&
                    newDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays -= direction;
                }
            }
            return newDate;
        }


        #region parsing definitions

        /// <summary>
        ///     Defines a substring where date-time was found and result of conversion
        /// </summary>
        public class ParsedDateTime
        {
            /// <summary>
            ///     Index of first char of a date substring found in the string
            /// </summary>
            public readonly int IndexOfDate = -1;

            /// <summary>
            ///     Length a date substring found in the string
            /// </summary>
            public readonly int LengthOfDate = -1;

            /// <summary>
            ///     Index of first char of a time substring found in the string
            /// </summary>
            public readonly int IndexOfTime = -1;

            /// <summary>
            ///     Length of a time substring found in the string
            /// </summary>
            public readonly int LengthOfTime = -1;

            /// <summary>
            ///     DateTime found in the string
            /// </summary>
            public readonly System.DateTime DateTime;

            /// <summary>
            ///     True if a date was found within the string
            /// </summary>
            public readonly bool IsDateFound;

            /// <summary>
            ///     True if a time was found within the string
            /// </summary>
            public readonly bool IsTimeFound;

            /// <summary>
            ///     UTC offset if it was found within the string
            /// </summary>
            public readonly TimeSpan UtcOffset;

            /// <summary>
            ///     True if UTC offset was found in the string
            /// </summary>
            public readonly bool IsUtcOffsetFound;

            /// <summary>
            ///     Utc gotten from DateTime if IsUtcOffsetFound is True
            /// </summary>
            public System.DateTime UtcDateTime;

            internal ParsedDateTime(int indexOfDate, int lengthOfDate, int indexOfTime, int lengthOfTime,
                System.DateTime dateTime)
            {
                IndexOfDate = indexOfDate;
                LengthOfDate = lengthOfDate;
                IndexOfTime = indexOfTime;
                LengthOfTime = lengthOfTime;
                DateTime = dateTime;
                IsDateFound = indexOfDate > -1;
                IsTimeFound = indexOfTime > -1;
                UtcOffset = new TimeSpan(25, 0, 0);
                IsUtcOffsetFound = false;
                UtcDateTime = new System.DateTime(1, 1, 1);
            }

            internal ParsedDateTime(int indexOfDate, int lengthOfDate, int indexOfTime, int lengthOfTime,
                System.DateTime dateTime, TimeSpan utcOffset)
            {
                IndexOfDate = indexOfDate;
                LengthOfDate = lengthOfDate;
                IndexOfTime = indexOfTime;
                LengthOfTime = lengthOfTime;
                DateTime = dateTime;
                IsDateFound = indexOfDate > -1;
                IsTimeFound = indexOfTime > -1;
                UtcOffset = utcOffset;
                IsUtcOffsetFound = System.Math.Abs(utcOffset.TotalHours) < 12;
                if (!IsUtcOffsetFound)
                {
                    UtcDateTime = new System.DateTime(1, 1, 1);
                }
                else
                {
                    if (indexOfDate < 0) //to avoid negative date exception when date is undefined
                    {
                        var ts = dateTime.TimeOfDay + utcOffset;
                        if (ts < new TimeSpan(0))
                            UtcDateTime = new System.DateTime(1, 1, 2) + ts;
                        else
                            UtcDateTime = new System.DateTime(1, 1, 1) + ts;
                    }
                    else
                    {
                        UtcDateTime = dateTime + utcOffset;
                    }
                }
            }
        }

        /// <summary>
        ///     Date that is accepted in the following cases:
        ///     - no date was parsed by TryParseDateOrTime();
        ///     - no year was found by TryParseDate();
        ///     It is ignored if DefaultDateIsNow = true was set after DefaultDate
        /// </summary>
        public static System.DateTime DefaultDate
        {
            set
            {
                _defaultDate = value;
                DefaultDateIsNow = false;
            }
            get
            {
                if (DefaultDateIsNow)
                    return System.DateTime.Now;
                return _defaultDate;
            }
        }

        private static System.DateTime _defaultDate = System.DateTime.Now;

        /// <summary>
        ///     If true then DefaultDate property is ignored and DefaultDate is always DateTime.Now
        /// </summary>
        public static bool DefaultDateIsNow = true;

        /// <summary>
        ///     Defines default date-time format.
        /// </summary>
        public enum DateTimeFormat
        {
            /// <summary>
            ///     month number goes before day number
            /// </summary>
            UsaDate,

            /// <summary>
            ///     day number goes before month number
            /// </summary>
            UkDate

            ///// <summary>
            ///// time is specifed through AM or PM
            ///// </summary>
            //USA_TIME,
        }

        #endregion

        #region parsing derived methods for DateTime output

        /// <summary>
        ///     Tries to find date and time within the passed string and return it as DateTime structure.
        /// </summary>
        /// <param name="str">string that contains date and/or time</param>
        /// <param name="defaultFormat">format to be used preferably in ambivalent instances</param>
        /// <param name="dateTime">parsed date-time output</param>
        /// <returns>true if both date and time were found, else false</returns>
        public static bool TryParseDateTime(this string str, DateTimeFormat defaultFormat, out System.DateTime dateTime)
        {
            if (!TryParseDateTime(str, defaultFormat, out ParsedDateTime parsedDateTime))
            {
                dateTime = new System.DateTime(1, 1, 1);
                return false;
            }

            dateTime = parsedDateTime.DateTime;
            return true;
        }

        /// <summary>
        ///     Tries to find date and/or time within the passed string and return it as DateTime structure.
        ///     If only date was found, time in the returned DateTime is always 0:0:0.
        ///     If only time was found, date in the returned DateTime is DefaultDate.
        /// </summary>
        /// <param name="str">string that contains date and(or) time</param>
        /// <param name="defaultFormat">format to be used preferably in ambivalent instances</param>
        /// <param name="dateTime">parsed date-time output</param>
        /// <returns>true if date and/or time was found, else false</returns>
        public static bool TryParseDateOrTime(this string str, DateTimeFormat defaultFormat, out System.DateTime dateTime)
        {
            if (!TryParseDateOrTime(str, defaultFormat, out ParsedDateTime parsedDateTime))
            {
                dateTime = new System.DateTime(1, 1, 1);
                return false;
            }

            dateTime = parsedDateTime.DateTime;
            return true;
        }

        /// <summary>
        ///     Tries to find time within the passed string and return it as DateTime structure.
        ///     It recognizes only time while ignoring date, so date in the returned DateTime is always 1/1/1.
        /// </summary>
        /// <param name="str">string that contains time</param>
        /// <param name="defaultFormat">format to be used preferably in ambivalent instances</param>
        /// <param name="time">parsed time output</param>
        /// <returns>true if time was found, else false</returns>
        public static bool TryParseTime(this string str, DateTimeFormat defaultFormat, out System.DateTime time)
        {
            if (!TryParseTime(str, defaultFormat, out var parsedTime, null))
            {
                time = new System.DateTime(1, 1, 1);
                return false;
            }

            time = parsedTime.DateTime;
            return true;
        }

        /// <summary>
        ///     Tries to find date within the passed string and return it as DateTime structure.
        ///     It recognizes only date while ignoring time, so time in the returned DateTime is always 0:0:0.
        ///     If year of the date was not found then it accepts the current year.
        /// </summary>
        /// <param name="str">string that contains date</param>
        /// <param name="defaultFormat">format to be used preferably in ambivalent instances</param>
        /// <param name="date">parsed date output</param>
        /// <returns>true if date was found, else false</returns>
        public static bool TryParseDate(this string str, DateTimeFormat defaultFormat, out System.DateTime date)
        {
            if (!TryParseDate(str, defaultFormat, out ParsedDateTime parsedDate))
            {
                date = new System.DateTime(1, 1, 1);
                return false;
            }

            date = parsedDate.DateTime;
            return true;
        }

        #endregion

        #region parsing derived methods for ParsedDateTime output

        /// <summary>
        ///     Tries to find date and time within the passed string and return it as ParsedDateTime object.
        /// </summary>
        /// <param name="str">string that contains date-time</param>
        /// <param name="defaultFormat">format to be used preferably in ambivalent instances</param>
        /// <param name="parsedDateTime">parsed date-time output</param>
        /// <returns>true if both date and time were found, else false</returns>
        public static bool TryParseDateTime(this string str, DateTimeFormat defaultFormat,
            out ParsedDateTime parsedDateTime)
        {
            if (str.TryParseDateOrTime(defaultFormat, out parsedDateTime)
                && parsedDateTime.IsDateFound
                && parsedDateTime.IsTimeFound
            )
                return true;

            parsedDateTime = null;
            return false;
        }

        /// <summary>
        ///     Tries to find time within the passed string and return it as ParsedDateTime object.
        ///     It recognizes only time while ignoring date, so date in the returned ParsedDateTime is always 1/1/1
        /// </summary>
        /// <param name="str">string that contains date-time</param>
        /// <param name="defaultFormat">format to be used preferably in ambivalent instances</param>
        /// <param name="parsedTime">parsed date-time output</param>
        /// <returns>true if time was found, else false</returns>
        public static bool TryParseTime(this string str, DateTimeFormat defaultFormat, out ParsedDateTime parsedTime)
        {
            return TryParseTime(str, defaultFormat, out parsedTime, null);
        }

        /// <summary>
        ///     Tries to find date and/or time within the passed string and return it as ParsedDateTime object.
        ///     If only date was found, time in the returned ParsedDateTime is always 0:0:0.
        ///     If only time was found, date in the returned ParsedDateTime is DefaultDate.
        /// </summary>
        /// <param name="str">string that contains date-time</param>
        /// <param name="defaultFormat">format to be used preferably in ambivalent instances</param>
        /// <param name="parsedDateTime">parsed date-time output</param>
        /// <returns>true if date or time was found, else false</returns>
        public static bool TryParseDateOrTime(this string str, DateTimeFormat defaultFormat,
            out ParsedDateTime parsedDateTime)
        {
            parsedDateTime = null;

            ParsedDateTime parsedTime;
            if (!TryParseDate(str, defaultFormat, out ParsedDateTime parsedDate))
            {
                if (!TryParseTime(str, defaultFormat, out parsedTime, null))
                    return false;

                var dateTime = new System.DateTime(DefaultDate.Year, DefaultDate.Month, DefaultDate.Day,
                    parsedTime.DateTime.Hour,
                    parsedTime.DateTime.Minute, parsedTime.DateTime.Second);
                parsedDateTime = new ParsedDateTime(-1, -1, parsedTime.IndexOfTime, parsedTime.LengthOfTime,
                    dateTime,
                    parsedTime.UtcOffset);
            }
            else
            {
                if (!TryParseTime(str, defaultFormat, out parsedTime, parsedDate))
                {
                    var dateTime = new System.DateTime(parsedDate.DateTime.Year, parsedDate.DateTime.Month,
                        parsedDate.DateTime.Day,
                        0, 0, 0);
                    parsedDateTime =
                        new ParsedDateTime(parsedDate.IndexOfDate, parsedDate.LengthOfDate, -1, -1, dateTime);
                }
                else
                {
                    var dateTime = new System.DateTime(parsedDate.DateTime.Year, parsedDate.DateTime.Month,
                        parsedDate.DateTime.Day,
                        parsedTime.DateTime.Hour, parsedTime.DateTime.Minute, parsedTime.DateTime.Second);
                    parsedDateTime = new ParsedDateTime(parsedDate.IndexOfDate, parsedDate.LengthOfDate,
                        parsedTime.IndexOfTime, parsedTime.LengthOfTime, dateTime, parsedTime.UtcOffset);
                }
            }

            return true;
        }

        #endregion

        #region parsing base methods

        /// <summary>
        ///     Tries to find time within the passed string (relatively to the passed parsed_date if any) and return it as
        ///     ParsedDateTime object.
        ///     It recognizes only time while ignoring date, so date in the returned ParsedDateTime is always 1/1/1
        /// </summary>
        /// <param name="str">string that contains date</param>
        /// <param name="defaultFormat">format to be used preferably in ambivalent instances</param>
        /// <param name="parsedTime">parsed date-time output</param>
        /// <param name="parsedDate">ParsedDateTime object if the date was found within this string, else NULL</param>
        /// <returns>true if time was found, else false</returns>
        public static bool TryParseTime(this string str, DateTimeFormat defaultFormat, out ParsedDateTime parsedTime,
            ParsedDateTime parsedDate)
        {
            parsedTime = null;

            string timeZoneR;
            if (defaultFormat == DateTimeFormat.UsaDate)
                timeZoneR = @"(?:\s*(?'time_zone'UTC|GMT|CST|EST))?";
            else
                timeZoneR = @"(?:\s*(?'time_zone'UTC|GMT))?";

            Match m;
            if (parsedDate != null && parsedDate.IndexOfDate > -1)
            {
                //look around the found date
                //look for <date> hh:mm:ss <UTC offset> 
                m = Regex.Match(str.Substring(parsedDate.IndexOfDate + parsedDate.LengthOfDate),
                    @"(?<=^\s*,?\s+|^\s*at\s*|^\s*[T\-]\s*)(?'hour'\d{2})\s*:\s*(?'minute'\d{2})\s*:\s*(?'second'\d{2})\s+(?'offset_sign'[\+\-])(?'offset_hh'\d{2}):?(?'offset_mm'\d{2})(?=$|[^\d\w])",
                    RegexOptions.Compiled);
                if (!m.Success)
                    //look for <date> [h]h:mm[:ss] [PM/AM] [UTC/GMT] 
                    m = Regex.Match(str.Substring(parsedDate.IndexOfDate + parsedDate.LengthOfDate),
                        @"(?<=^\s*,?\s+|^\s*at\s*|^\s*[T\-]\s*)(?'hour'\d{1,2})\s*:\s*(?'minute'\d{2})\s*(?::\s*(?'second'\d{2}))?(?:\s*(?'ampm'AM|am|PM|pm))?" +
                        timeZoneR + @"(?=$|[^\d\w])", RegexOptions.Compiled);
                if (!m.Success)
                    //look for [h]h:mm:ss [PM/AM] [UTC/GMT] <date>
                    m = Regex.Match(str.Substring(0, parsedDate.IndexOfDate),
                        @"(?<=^|[^\d])(?'hour'\d{1,2})\s*:\s*(?'minute'\d{2})\s*(?::\s*(?'second'\d{2}))?(?:\s*(?'ampm'AM|am|PM|pm))?" +
                        timeZoneR + @"(?=$|[\s,]+)", RegexOptions.Compiled);
                if (!m.Success)
                    //look for [h]h:mm:ss [PM/AM] [UTC/GMT] within <date>
                    m = Regex.Match(str.Substring(parsedDate.IndexOfDate, parsedDate.LengthOfDate),
                        @"(?<=^|[^\d])(?'hour'\d{1,2})\s*:\s*(?'minute'\d{2})\s*(?::\s*(?'second'\d{2}))?(?:\s*(?'ampm'AM|am|PM|pm))?" +
                        timeZoneR + @"(?=$|[\s,]+)", RegexOptions.Compiled);
            }
            else //look anywhere within string
            {
                //look for hh:mm:ss <UTC offset> 
                m = Regex.Match(str,
                    @"(?<=^|\s+|\s*T\s*)(?'hour'\d{2})\s*:\s*(?'minute'\d{2})\s*:\s*(?'second'\d{2})\s+(?'offset_sign'[\+\-])(?'offset_hh'\d{2}):?(?'offset_mm'\d{2})?(?=$|[^\d\w])",
                    RegexOptions.Compiled);
                if (!m.Success)
                    //look for [h]h:mm[:ss] [PM/AM] [UTC/GMT]
                    m = Regex.Match(str,
                        @"(?<=^|\s+|\s*T\s*)(?'hour'\d{1,2})\s*:\s*(?'minute'\d{2})\s*(?::\s*(?'second'\d{2}))?(?:\s*(?'ampm'AM|am|PM|pm))?" +
                        timeZoneR + @"(?=$|[^\d\w])", RegexOptions.Compiled);
            }

            if (!m.Success)
                return false;

            //try
            //{
            var hour = int.Parse(m.Groups["hour"].Value);
            if (hour < 0 || hour > 23)
                return false;

            var minute = int.Parse(m.Groups["minute"].Value);
            if (minute < 0 || minute > 59)
                return false;

            var second = 0;
            if (!string.IsNullOrEmpty(m.Groups["second"].Value))
            {
                second = int.Parse(m.Groups["second"].Value);
                if (second < 0 || second > 59)
                    return false;
            }

            if (string.Compare(m.Groups["ampm"].Value, "PM", StringComparison.OrdinalIgnoreCase) == 0 && hour < 12)
                hour += 12;
            else if (string.Compare(m.Groups["ampm"].Value, "AM", StringComparison.OrdinalIgnoreCase) == 0 &&
                     hour == 12)
                hour -= 12;

            var dateTime = new System.DateTime(1, 1, 1, hour, minute, second);

            if (m.Groups["offset_hh"].Success)
            {
                var offsetHh = int.Parse(m.Groups["offset_hh"].Value);
                var offsetMm = 0;
                if (m.Groups["offset_mm"].Success)
                    offsetMm = int.Parse(m.Groups["offset_mm"].Value);
                var utcOffset = new TimeSpan(offsetHh, offsetMm, 0);
                if (m.Groups["offset_sign"].Value == "-")
                    utcOffset = -utcOffset;
                parsedTime = new ParsedDateTime(-1, -1, m.Index, m.Length, dateTime, utcOffset);
                return true;
            }

            if (m.Groups["time_zone"].Success)
            {
                TimeSpan utcOffset;
                switch (m.Groups["time_zone"].Value)
                {
                    case "UTC":
                    case "GMT":
                        utcOffset = new TimeSpan(0, 0, 0);
                        break;
                    case "CST":
                        utcOffset = new TimeSpan(-6, 0, 0);
                        break;
                    case "EST":
                        utcOffset = new TimeSpan(-5, 0, 0);
                        break;
                    default:
                        throw new Exception("Time zone: " + m.Groups["time_zone"].Value + " is not defined.");
                }

                parsedTime = new ParsedDateTime(-1, -1, m.Index, m.Length, dateTime, utcOffset);
                return true;
            }

            parsedTime = new ParsedDateTime(-1, -1, m.Index, m.Length, dateTime);
            //}
            //catch(Exception e)
            //{
            //    return false;
            //}
            return true;
        }

        /// <summary>
        ///     Tries to find date within the passed string and return it as ParsedDateTime object.
        ///     It recognizes only date while ignoring time, so time in the returned ParsedDateTime is always 0:0:0.
        ///     If year of the date was not found then it accepts the current year.
        /// </summary>
        /// <param name="str">string that contains date</param>
        /// <param name="defaultFormat">format to be used preferably in ambivalent instances</param>
        /// <param name="parsedDate">parsed date output</param>
        /// <returns>true if date was found, else false</returns>
        public static bool TryParseDate(this string str, DateTimeFormat defaultFormat, out ParsedDateTime parsedDate)
        {
            parsedDate = null;

            if (string.IsNullOrEmpty(str))
                return false;

            //look for dd/mm/yy
            var m = Regex.Match(str,
                @"(?<=^|[^\d])(?'day'\d{1,2})\s*(?'separator'[\\/\.])+\s*(?'month'\d{1,2})\s*\'separator'+\s*(?'year'\d{2}|\d{4})(?=$|[^\d])",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                System.DateTime date;
                if ((defaultFormat ^ DateTimeFormat.UsaDate) == DateTimeFormat.UsaDate)
                {
                    if (!ConvertToDate(int.Parse(m.Groups["year"].Value), int.Parse(m.Groups["day"].Value),
                        int.Parse(m.Groups["month"].Value), out date))
                        return false;
                }
                else
                {
                    if (!ConvertToDate(int.Parse(m.Groups["year"].Value), int.Parse(m.Groups["month"].Value),
                        int.Parse(m.Groups["day"].Value), out date))
                        return false;
                }

                parsedDate = new ParsedDateTime(m.Index, m.Length, -1, -1, date);
                return true;
            }

            //look for [yy]yy-mm-dd
            m = Regex.Match(str,
                @"(?<=^|[^\d])(?'year'\d{2}|\d{4})\s*(?'separator'[\-])\s*(?'month'\d{1,2})\s*\'separator'+\s*(?'day'\d{1,2})(?=$|[^\d])",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                System.DateTime date;
                if (!ConvertToDate(int.Parse(m.Groups["year"].Value), int.Parse(m.Groups["month"].Value),
                    int.Parse(m.Groups["day"].Value), out date))
                    return false;
                parsedDate = new ParsedDateTime(m.Index, m.Length, -1, -1, date);
                return true;
            }

            //look for month dd yyyy
            m = Regex.Match(str,
                @"(?:^|[^\d\w])(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*\s+(?'day'\d{1,2})(?:-?st|-?th|-?rd|-?nd)?\s*,?\s*(?'year'\d{4})(?=$|[^\d\w])",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!m.Success)
                //look for dd month [yy]yy
                m = Regex.Match(str,
                    @"(?:^|[^\d\w:])(?'day'\d{1,2})(?:-?st\s+|-?th\s+|-?rd\s+|-?nd\s+|-|\s+)(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*(?:\s*,?\s*|-)'?(?'year'\d{2}|\d{4})(?=$|[^\d\w])",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!m.Success)
                //look for yyyy month dd
                m = Regex.Match(str,
                    @"(?:^|[^\d\w])(?'year'\d{4})\s+(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*\s+(?'day'\d{1,2})(?:-?st|-?th|-?rd|-?nd)?(?=$|[^\d\w])",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!m.Success)
                //look for month dd hh:mm:ss MDT|UTC yyyy
                m = Regex.Match(str,
                    @"(?:^|[^\d\w])(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*\s+(?'day'\d{1,2})\s+\d{2}\:\d{2}\:\d{2}\s+(?:MDT|UTC)\s+(?'year'\d{4})(?=$|[^\d\w])",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!m.Success)
                //look for  month dd [yyyy]
                m = Regex.Match(str,
                    @"(?:^|[^\d\w])(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*\s+(?'day'\d{1,2})(?:-?st|-?th|-?rd|-?nd)?(?:\s*,?\s*(?'year'\d{4}))?(?=$|[^\d\w])",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                var month = -1;
                var index_of_date = m.Index;
                var length_of_date = m.Length;

                switch (m.Groups["month"].Value)
                {
                    case "Jan":
                    case "JAN":
                        month = 1;
                        break;
                    case "Feb":
                    case "FEB":
                        month = 2;
                        break;
                    case "Mar":
                    case "MAR":
                        month = 3;
                        break;
                    case "Apr":
                    case "APR":
                        month = 4;
                        break;
                    case "May":
                    case "MAY":
                        month = 5;
                        break;
                    case "Jun":
                    case "JUN":
                        month = 6;
                        break;
                    case "Jul":
                        month = 7;
                        break;
                    case "Aug":
                    case "AUG":
                        month = 8;
                        break;
                    case "Sep":
                    case "SEP":
                        month = 9;
                        break;
                    case "Oct":
                    case "OCT":
                        month = 10;
                        break;
                    case "Nov":
                    case "NOV":
                        month = 11;
                        break;
                    case "Dec":
                    case "DEC":
                        month = 12;
                        break;
                }

                var year = !string.IsNullOrEmpty(m.Groups["year"].Value) ? int.Parse(m.Groups["year"].Value) : DefaultDate.Year;

                if (!ConvertToDate(year, month, int.Parse(m.Groups["day"].Value), out var date))
                    return false;
                parsedDate = new ParsedDateTime(index_of_date, length_of_date, -1, -1, date);
                return true;
            }

            return false;
        }

        public static bool ConvertToDate(int year, int month, int day, out System.DateTime date)
        {
            if (year >= 100)
            {
                if (year < 1000)
                {
                    date = new System.DateTime(1, 1, 1);
                    return false;
                }
            }
            else if (year > 30)
            {
                year += 1900;
            }
            else
            {
                year += 2000;
            }

            try
            {
                date = new System.DateTime(year, month, day);
            }
            catch
            {
                date = new System.DateTime(1, 1, 1);
                return false;
            }

            return true;
        }

        #endregion
    }
}