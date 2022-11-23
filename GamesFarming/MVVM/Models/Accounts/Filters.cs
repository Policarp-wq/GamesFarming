using GamesFarming.DataBase;
using System;

namespace GamesFarming.MVVM.Models.Accounts
{
    internal static class Filters
    {
        public static Func<Account, bool> GetGeneralFilter(string s)
        {
            if(s is null || s.Length < 1)
                return null;
            return account => GetLoginFilter(s)(account) || GetGameFilter(s)(account);
        }

        public static Func<Account, bool> GetLoginFilter(string s)
        {
            if (s is null || s.Length < 1)
                return null;
            return account => account.Login.Contains(s);
        }
        public static Func<Account, bool> GetGameFilter(string s)
        {
            if (s is null || s.Length < 1)
                return null;
            return account => Decoder.GetName(account.GameCode).Contains(s) || account.GameCode.ToString().Contains(s);
        }
    }
}
