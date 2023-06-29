using GamesFarming.User;
using System;

namespace GamesFarming.MVVM.Models.Steam
{
    public static class LaunchTimeManager
    {
        public const int DefaultHoursFarming = 6;
        public const int DefaultMinutesFarming = 20;
        public static int HoursFarming => UserSettings.GetFarmTimeHours();
        public static int MinutesFarming => UserSettings.GetFarmTimeMinutes();
        public static int DaysSpanCS = 7;
        public static int DaysSpanTF = 7;
        public static TimeSpan LaunchSpanCS => new TimeSpan(DaysSpanCS, HoursFarming, MinutesFarming, 0);
        public static TimeSpan LaunchSpanTF => new TimeSpan(DaysSpanTF, HoursFarming, MinutesFarming, 0);
        public static TimeSpan FarmingTime => new TimeSpan(HoursFarming, MinutesFarming, 0);
        public static DateTime Today => DateTime.Now;

        public static int FarmingSeconds => (int)FarmingTime.TotalSeconds;

        public static bool NeedToLaunch(DateTime last, int gameCode)
        {
            switch (gameCode)
            {
                case 440:
                    return DateTime.Now - last >= LaunchSpanTF;
                default:
                    return NeedToLaunchCS(last);
            }
           
        }

        private static bool NeedToLaunchCS(DateTime last)
        {
            last = new DateTime(last.Year, last.Month, last.Day);
            DateTime nextWednesday = last.AddDays((7 + DayOfWeek.Wednesday - last.DayOfWeek) % 7);
            if(last.DayOfWeek == DayOfWeek.Wednesday)
            {
                nextWednesday = nextWednesday.AddDays(7);
            }
            return Today.Date >= nextWednesday;
        }

        //private static bool NeedToLaunchCS(DateTime last)
        //{
        //    if (last.DayOfWeek < DayOfWeek.Wednesday && Today.DayOfWeek >= DayOfWeek.Wednesday)
        //        return true;
        //    else if (last.DayOfWeek > DayOfWeek.Wednesday && Today - last >= TimeSpan.FromDays(7))
        //        return true;
        //    else if (last.DayOfWeek == DayOfWeek.Wednesday)
        //        return last.Hour < 6;
        //    return false;
        //}

    }
}
