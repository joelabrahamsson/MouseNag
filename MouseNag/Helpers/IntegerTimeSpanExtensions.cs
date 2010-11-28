using System;

namespace MouseNag.Helpers
{
    public static class IntegerTimeSpanExtensions
    {
        public static TimeSpan Milliseconds(this int number)
        {
            return new TimeSpan(0, 0, 0, 0, number);
        }
    }
}
