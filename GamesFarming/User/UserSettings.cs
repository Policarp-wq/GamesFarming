﻿using System;
using GamesFarming.MVVM.Models;
using Newtonsoft.Json;

namespace GamesFarming.User
{
    public static class UserSettings
    {
        private class Settings
        {
            public string SteamPath = "";

        }
        private static Settings _settings;

        public static string SettingsName = "UserSettings";
        public static string SettingsFolder;
        public static string SettingsPath => SettingsFolder + SettingsName;
        public static bool ContainsSteamPath => GetSteamPath().Length > 0;


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

        private static void Save()
        {
            string serializedSettings = JsonConvert.SerializeObject(_settings);
            Write(serializedSettings);
        }

        private static Settings GetSettings()
        {
            var serialized = Read();
            var settings = JsonConvert.DeserializeObject<Settings>(serialized);
            if (settings is null)
                return new Settings();
            return settings;
        }
        private static string Read()
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
