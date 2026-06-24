using System;
using System.Globalization;

namespace Project.Core.Extension
{
    public static class DateExtension
    {
        public static string GetScriptDate(DateTime dt)
        {
            if (dt == null)
            {
                return string.Empty;
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }
        }

        public static DateTime GetUtcDateTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public static string GetScriptDateDMY(DateTime dt)
        {
            if (dt == null)
            {
                return string.Empty;
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }
        }

        public static string GetScriptDateDMYStr(this DateTime date) => date.ConvertUtcToUkTime().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

       

        public static DateTime GetFirstDateOfMonth(this DateTime date)
        {
            if (date == null)
            {
                return date;
            }
            else
            {
                return new DateTime(date.Year, date.Month, 1);
            }
        }


        public static string GetDateTimeStringWithTime(this DateTime date)
        {
            if (date == null)
            {
                return string.Empty;
            }
            else
            {
                //return date.ToLocalTime().ToString("yyyy-MM-dd HH:mm tt");
                return date.ConvertUtcToUkTime().ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
        }

        public static string GetDateTimeStringWithTimeWithoutSecond(this DateTime date) => date.ConvertUtcToUkTime().ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);



        public static string GetDateTimeStringWithTimeMMM(this DateTime date)
        {
            if (date == null)
            {
                return string.Empty;
            }
            else
            {
                //return date.ToLocalTime().ToString("yyyy-MM-dd HH:mm tt");
                return date.ConvertUtcToUkTime().ToString("dd MMM yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }
        }

        public static string GetLocalDateTime(this DateTime date)
        {
            if (date == null)
            {
                return string.Empty;
            }
            else
            {
                //return date.ToLocalTime().ToString("yyyy-MM-dd HH:mm tt");
                return date.ToLocalTime().ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
        }

        public static DateTime ConvertUtcToUkTime(this DateTime utcDateTime)
        {
            TimeZoneInfo ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            DateTime ukDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, ukTimeZone);
            return ukDateTime;
        }
    }
}
