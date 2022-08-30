using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesFarming.User
{
    public static class UserSettings
    {
        public static string SettingsName = "UserSettings";
        public static string SettingsPath => Path.GetTempPath() + SettingsName;
        public static string SteamPath => File.ReadAllText(SettingsPath);
        public static bool ContainsSteamPath => File.Exists(SettingsPath);

        public static void SetSteamPath(string steamPath)
        {
            File.WriteAllText(SettingsPath, steamPath);
        }
    }
}
