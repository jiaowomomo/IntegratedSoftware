
using System;

namespace CommonLibrary.ExtensionUtils
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// 获取系统启动时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSystemBootDateTime()
        {
            return DateTime.Now.AddMilliseconds(-Environment.TickCount);
        }

        public static string ToDisplayString(this DateTime instance, string format)
        {
            return instance == DateTime.MinValue ? "(无)" : instance.ToString(format);
        }

        public static string ToDisplayString(this DateTime instance, bool isContainSecond = true)
        {
            var format = "yyyy-MM-dd HH:mm";
            if (isContainSecond) {
                format = "yyyy-MM-dd HH:mm:ss";
            }
            return instance == DateTime.MinValue ? "(无)" : instance.ToString(format);
        }

        ///   <summary>   
        ///   取指定日期是一年中的第几周   
        ///   </summary>   
        ///   <param   name="dateTime">给定的日期</param>   
        ///   <returns>返回 该日期所在一年中的周数</returns>   
        public static int GetWeekOfYear(this DateTime instance)
        {
            int firstdayofweek = System.Convert.ToDateTime(instance.Year.ToString() + "- " + "1-1 ").DayOfWeek.GetHashCode();
            int days = instance.DayOfYear;
            int daysOutOneWeek = days - (7 - firstdayofweek);
            if (daysOutOneWeek <= 0)
            {
                return 1;
            }
            else
            {
                int weeks = daysOutOneWeek / 7;
                if (daysOutOneWeek % 7 != 0)
                {
                    weeks++;
                }
                return weeks + 1;
            }
        }

        /// <summary>
        /// 是否闰年
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsLeapYear(this DateTime instance)
        {
            if ((instance.Year % 400 == 0 && instance.Year % 3200 != 0)
               || (instance.Year % 4 == 0 && instance.Year % 100 != 0)
               || (instance.Year % 3200 == 0 && instance.Year % 172800 == 0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取指定周时间段
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekIndex"></param>
        /// <returns></returns>
        public static DateRange GetWeekRange(int year, int weekIndex)
        {
            weekIndex += 1;
            try
            {
                if (weekIndex < 1)
                {
                    throw new Exception("请输入大于0的整数");
                }

                int allDays = (weekIndex - 1) * 7;
                //确定当年第一天
                DateTime firstDate = new DateTime(year, 1, 1);
                int firstDayOfWeek = (int)firstDate.DayOfWeek;
                firstDayOfWeek = firstDayOfWeek == 0 ? 7 : firstDayOfWeek;
                //周开始日
                int startAddDays = allDays + (1 - firstDayOfWeek);
                DateTime weekRangeStart = firstDate.AddDays(startAddDays);
                //周结束日
                int endAddDays = allDays + (7 - firstDayOfWeek);
                DateTime weekRangeEnd = firstDate.AddDays(endAddDays);
                if (weekRangeStart.Year > year ||
                 (weekRangeStart.Year == year && weekRangeEnd.Year > year))
                {
                    throw new Exception("今年没有第" + weekIndex + "周。");
                }
                var dr = new DateRange();
                dr.Start = weekRangeStart.AddDays(-1);
                dr.End = weekRangeEnd.AddDays(-1);
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public struct DateRange
    {
        public DateTime Start;
        public DateTime End;
    }
}
