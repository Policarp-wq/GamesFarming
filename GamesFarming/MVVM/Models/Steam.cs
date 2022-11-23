using GamesFarming.User;
using System;
using System.Drawing;
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
        public static int DefaultSteamLaunchMilliSeconds = DefaultSteamLaunchSeconds * 1000;
        public const int MilliSecondsAfterLaucnh = 2000;
        public const int DefaultSteamLaunchSeconds = 10;
        public static int SteamLaunchSeconds => UserSettings.GetLaunchSeconds();
        public static int SteamLaunchMilliSeconds => SteamLaunchSeconds * 1000;
        public static int SteamAutorizationSeconds = 25;
        public static int SteamAutorizationMilliSeconds => SteamAutorizationSeconds * 1000;
        public static int CloudCleanSeconds = 110;
        public static int CloudCleanMilliSeconds => CloudCleanSeconds * 1000;
        public static int SteamQuitWindowAwait = 20;
        public static int SteamQuitWindowAwaitMilliSeconds => SteamQuitWindowAwait * 1000;

        public const string GuardName = "guard.exe";
        public const string CleanerName = "cloudCleaner.exe";
        public const string ResolutionParameter = "mat_setvideomode";
        public static string GuardPath => Environment.CurrentDirectory + "\\" + GuardName;
        public static string CleanerPath => Environment.CurrentDirectory + "\\" + CleanerName;
        public static Color TickColor = Color.FromArgb(0, 43, 47, 52);

        public static string GetSteamFolderPath()
        {
            var steamPath = UserSettings.GetSteamPath();
            var a = steamPath.Split('\\').Where(x => !x.Contains(".exe"));
            return string.Join("\\", a);
        }

    }
}
