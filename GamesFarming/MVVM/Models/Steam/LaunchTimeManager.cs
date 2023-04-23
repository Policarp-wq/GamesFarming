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

        public static int FarmingSeconds => (int)FarmingTime.TotalSeconds;

        public static bool NeedToLaunch(DateTime last, int gameCode)
        {
            switch (gameCode)
            {
                case 440:
                    return DateTime.Now - last >= LaunchSpanTF;
                default:
                    return DateTime.Now - last >= LaunchSpanCS;
            }
           
        }
    }
}
