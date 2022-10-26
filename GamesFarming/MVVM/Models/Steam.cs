using GamesFarming.User;
using System;
using System.Linq;

namespace GamesFarming.MVVM.Models
{
    public static class Steam
    {
        public static int AccuracyMinutes = 20;
        public static int DaysSpan = 7;
        public static int HoursFarming = 6;
        public static TimeSpan LaunchSpan => new TimeSpan(DaysSpan, HoursFarming, AccuracyMinutes, 0);
        public static TimeSpan FarmingTime => new TimeSpan(HoursFarming, AccuracyMinutes, 0);
        public const int MilliSecondsAwaitSteam = DefaultSteamLaunchSeconds * 1000;
        public const int MilliSecondsAfterLaucnh = 1000;
        public const int DefaultSteamLaunchSeconds = 10;

        public const string GuardName = "guard.exe";
        public const string ResolutionParameter = "mat_setvideomode";
        public static string GuardPath => Environment.CurrentDirectory + "\\" + GuardName;

        public static string GetSteamFolderPath()
        {
            var steamPath = UserSettings.GetSteamPath();
            var a = steamPath.Split('\\').Where(x => !x.Contains(".exe"));
            return String.Join("\\", a);
        }

    }
}
