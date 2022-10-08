using System;

namespace GamesFarming.MVVM.Models
{
    public static class Steam
    {
        public static int AccuracyMinutes = 20;
        public static int DaysSpan = 7;
        public static int HoursFarming = 5;
        public static TimeSpan LaunchSpan => new TimeSpan(DaysSpan, HoursFarming, AccuracyMinutes, 0);
        public static TimeSpan FarmingTime => new TimeSpan(HoursFarming, AccuracyMinutes, 0);
        public const string GuardName = "guard.exe";
        public static string GuardPath => Environment.CurrentDirectory + "\\" + GuardName;
    }
}
