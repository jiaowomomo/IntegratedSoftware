using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    public static class TimeSpanUtils
    {
        public static TimeSpan FromString(string timeString)
        {
            var tsArray = timeString.Split(':');
            return new TimeSpan(tsArray[0].ToInt32(), tsArray[1].ToInt32(), tsArray[2].ToInt32());
        }
    }
}
