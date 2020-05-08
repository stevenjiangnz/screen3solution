using System;
using System.IO;
using System.Collections.Generic;

namespace Screen3.Utils
{
    public class DateHelper
    {
        public static DateTime ToDate(int period)
        {
            int year = int.Parse(period.ToString().Substring(0, 4));
            int month = int.Parse(period.ToString().Substring(4, 2));
            int day = int.Parse(period.ToString().Substring(6, 2));
            DateTime dt = new DateTime(year, month, day);

            return dt;
        }

        public static int ToInt(DateTime dt)
        {
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;

            return year * 10000 + month * 100 + day;
        }


        public static int EndOfWeek(int intDate, DayOfWeek endOfWeek = DayOfWeek.Friday)
        {
            DateTime dt = ToDate(intDate);

            while (dt.DayOfWeek != endOfWeek)
            {
                dt = dt.AddDays(1);
            }

            return ToInt(dt);
        }

    }
}