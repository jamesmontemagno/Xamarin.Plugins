#if __UNIFIED__
using Foundation;
#else
using MonoTouch.Foundation;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Geolocator
{
    public static class DateHelpers
    {

        public static DateTime ToDateTime(this NSDate date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return reference.AddSeconds(date.SecondsSinceReferenceDate);
        }


        public static NSDate ToNSDate(this DateTime date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - reference).TotalSeconds);
        }
    }
}
