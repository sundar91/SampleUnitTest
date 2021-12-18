namespace eBroker.Services
{
    public class Utility
    {
        public static bool IsValidDuration(DateTime now)
        {
            if(now.TimeOfDay > new TimeSpan(9,0,0) &&
                now.TimeOfDay < new TimeSpan(15,0,0))
            {
                return true;
            }

            return false;
        }

        public static bool IsValidDayOfWeek(DateTime now)
        {
            var week = (int)now.DayOfWeek;
            if(week >= 1 && week <= 5 )
                return true;

            return false;
        }

    }
}
