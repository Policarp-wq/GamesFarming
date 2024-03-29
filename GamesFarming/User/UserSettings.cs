﻿using System;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Models.Steam;
using Newtonsoft.Json;

namespace GamesFarming.User
{
    public static class UserSettings
    {
        private class Settings
        {
            public string SteamPath = "";
            public string CfgPath = "";
            public string MAFilesPath = "";
            public int LaunchSeconds = SteamLibrary.DefaultSteamLaunchSeconds; 
            public int FarmTimeHrs = LaunchTimeManager.DefaultHoursFarming; 
            public int FarmTimeMins = LaunchTimeManager.DefaultMinutesFarming; 
            public int AccsInGroup = SteamLibrary.DefaultAccsInGroup; 

        }
        private static readonly Settings _settings;

        public static string SettingsName = "UserSettings";
        public static string MAFilesName = "MAFilesPathHolder.txt";

        public static string SettingsFolder;
        public static string SettingsPath => SettingsFolder + SettingsName;
        private static string MAFilesPathHolder => SettingsFolder + MAFilesName;
        public static bool ContainsSteamPath => GetSteamPath().Length > 0;
        public static bool ContainsMAFilesPath => GetMAFilesPath().Length > 0;
        public static bool ContainsCfgPath => GetCfgPath().Length > 0;


        static UserSettings()
        {
            SettingsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
            _settings = GetSettings();
        }

        public static void SetSteamPath(string steamPath)
        {
            _settings.SteamPath = steamPath;
            Save();
        }

        public static string GetSteamPath() => _settings.SteamPath;

        public static void SetCfgPath(string cfgPath)
        {
            _settings.CfgPath = cfgPath;
            Save();
        }

        public static string GetCfgPath() => _settings.CfgPath;
        public static int GetLaunchSeconds() => _settings.LaunchSeconds;

        public static void SetLaunchSeconds(int sec)
        {
            _settings.LaunchSeconds = sec;
            Save();
        }

        public static int GetAccsInGroup() => _settings.AccsInGroup;
        public static void SetAccsInGroup(int cnt)
        {
            _settings.AccsInGroup = cnt;
            Save();
        }

        public static int GetFarmTimeHours() => _settings.FarmTimeHrs;
        public static void SetFarmTimeHours(int hours)
        {
            _settings.FarmTimeHrs = hours;
            Save();
        }

        public static int GetFarmTimeMinutes() => _settings.FarmTimeMins;
        public static void SetFarmTimeMinutes(int mins)
        {
            _settings.FarmTimeMins = mins;
            Save();
        }


        public static void SetMAFilesPath(string maFilesFolderPath)
        {
            FileSafeAccess.WriteToFile(MAFilesPathHolder, maFilesFolderPath);
        }
        public static string GetMAFilesPath()
        {
            return FileSafeAccess.ReadFile(MAFilesPathHolder);
        }

        private static void Save()
        {
            string serializedSettings = JsonConvert.SerializeObject(_settings);
            Write(serializedSettings);
        }

        private static Settings GetSettings()
        {
            var serialized = GetSerializedSettings();
            var settings = JsonConvert.DeserializeObject<Settings>(serialized);
            if (settings is null)
                return new Settings();
            return settings;
        }
        
        private static string GetSerializedSettings()
        {
            var serialized = FileSafeAccess.ReadFile(SettingsPath);
            return serialized;
        }
        private static void Write(string text)
        {
            FileSafeAccess.WriteToFile(SettingsPath, text);
        }
        
    }
}
