using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DateTimeExtensions
    {
        public static string RelativeDate(this DateTime date)
        {
            var timespan = DateTime.Now.Subtract(date);
            if (timespan.Days > 1)
                return date.ToString();
            if (timespan.Hours > 1)
                return string.Format("Over {0} hours ago.", timespan.Hours);
            if (timespan.Minutes > 1)
                return string.Format("{0} minutes ago.", timespan.Minutes);
            return string.Format("{0} seconds ago.", timespan.Seconds);
        }

        public static int DaysLeft(this DateTime date)
        {
            return DateTime.Today.Subtract(date).Days;
        }

        public static int HolidayDaysLeft(this DateTime date)
        {
            var xdate = new DateTime(date.Year, date.Month, date.Day);

            if (date.Date <= DateTime.Today.Date)
            {
                xdate = xdate.AddYears(1);
            }

            return xdate.DaysLeft();
        }
    }
}
