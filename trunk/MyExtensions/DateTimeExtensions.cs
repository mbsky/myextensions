using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace System
{
    public static class DateTimeExtensions
    {
        public static string RelativeDate(this DateTime date)
        {
            var timespan = DateTime.Now.Subtract(date);

            //check Accept-Language 

            var acceptLang = string.Empty;

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                acceptLang = HttpContext.Current.Request.Headers["Accept-Language"];
            }

            if (timespan.Days > 1)
            {
                switch (acceptLang)
                {
                    default:
                        return date.ToString();
                    case "zh-CN":
                        return date.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            if (timespan.Hours > 1)
            {
                string tpl = "Over {0} hours ago.";

                switch (acceptLang)
                {
                    case "zh-CN":
                        tpl = "{0}小时以前。";
                        break;
                }

                return string.Format(tpl, timespan.Hours);
            }

            if (timespan.Minutes > 1)
            {
                string tpl = "Over {0} minutes ago.";

                switch (acceptLang)
                {
                    case "zh-CN":
                        tpl = "{0}分钟以前。";
                        break;
                }

                return string.Format(tpl, timespan.Minutes);
            }
            else
            {
                string tpl = "{0} seconds ago.";

                switch (acceptLang)
                {
                    case "zh-CN":
                        tpl = "{0}秒以前。";
                        break;
                }

                return string.Format(tpl, timespan.Seconds);
            }
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
