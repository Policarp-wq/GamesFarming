using GamesFarming.DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GamesFarming.MVVM.Models
{
    public static class BatDeserializer
    {
        // KeyWords for seeking
        public const string Login = "-login";
        public const string Launch = "-applaunch";
        public const string Width = "-w";
        public const string Height = "-h";
        public const string Execution = "+exec";
        public const string ConnectIP = "connect";
        public const string ConnectPass = "password";

        public static Account Deserialize(string path)
        {
            var arg = FileSafeAccess.ReadFile(path);
            if (arg is null)
                throw new ArgumentException("Wrong path to the file! :" + path);
            char[] charsToTrim = { ';', '\"', '.', ',' };
            var args = arg.Split(' ').Select(s => s.Trim(charsToTrim)).ToArray();
            try
            {
                return GetAccountFromArgs(args, path);
            }
            catch(Exception ex)
            {
                throw new FileFormatException("Wrong .bat format! : " + ex.Message);
            }
        }

        public static IEnumerable<Account> DeserializeFolderFiles(string folderPath, Func<string, Exception, bool> onException = null)
        {
            var arg = FileSafeAccess.ReadFile(folderPath);
            if (arg is null)
                throw new ArgumentException("Wrong path to the folder! :" + folderPath);
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                Account deserialized;
                if (file.Contains(".bat"))
                {
                    try
                    {
                        deserialized = Deserialize(file);
                    }
                    catch (Exception ex)
                    {
                        if(onException is null)
                            throw new FileFormatException("Error was caused while deserializing the file : " + file + " || Error : " + ex.Message);
                        if (onException(file, ex))
                            continue;
                        else yield break;
                    }
                    yield return deserialized;
                }
                        

            }
        }
        private static Account GetAccountFromArgs(string[] args, string path)
        {
            var AccLogin = GetArg(args, Login);
            var AccPassword = GetPassword(args);
            var AccResolution = GetResolution(args);
            var AccGameCode = GetGameCode(args);
            var AccOptimization = LaunchArgument.DefaultOptimization;
            var Cfg = GetArg(args, Execution);
            Account account = new Account(AccLogin, AccPassword, AccGameCode, AccResolution, Cfg, AccOptimization);
            if (!Validator.IsAccountValid(account))
                throw new FileFormatException("Invalid bat to convert! Path : " + path + "Deserialized account : " + account.ToString());
            account.Connect = GetConnect(args);
            return account;
        }

        private static int GetInd(string[] args, string expect)
            => Array.IndexOf(args, expect);

        private static string GetArg(string[] args, string argName)
        {
            var ind = GetInd(args, argName);
            if (ind == -1 || ind + 1 >= args.Length)
                return null;
            return args[ind + 1];
        }

        private static string GetPassword(string[] args)
        {
            var ind = GetInd(args, Login);
            if (ind == -1 || ind + 2 >= args.Length)
                return null;
            return args[ind + 2];
        }

        private static Resolution GetResolution(string[] args)
        {
            var width = GetArg(args, Width);
            var height = GetArg(args, Height);
            if (!(int.TryParse(width, out int w) && int.TryParse(height, out int h)))
                return Resolution.Default;
            return new Resolution(h, w);
        }

        private static int GetGameCode(string[] args)
        {
            if (!int.TryParse(GetArg(args, Launch), out int code))
                return LaunchArgument.DefaultCode;
            return code;
        }

        private static Connection GetConnect(string[] args)
        {
            var ip = GetArg(args, ConnectIP);
            var password = GetArg(args, ConnectPass);
            if (ip is null)
                return null;
            return new Connection(ip, password);
        }

    }
}
