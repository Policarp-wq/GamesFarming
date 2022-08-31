using GamesFarming.MVVM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesFarming.DataBase
{
    public static class JsonDB
    {
        public static string UsersFileName = "UsersDB";
        public static string FolderPath;
        public static string DBPath => FolderPath + UsersFileName;
        static JsonDB()
        {
            FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
        }

        public static void WriteToDB(Account account)
        {
            var serializedAccounts = GetAcounts();
            serializedAccounts.Add(account);

            string serializedUsers = JsonConvert.SerializeObject(serializedAccounts);
            Write(serializedUsers);

        }
        public static void WriteToDB(IEnumerable<Account> accounts)
        {
            var serializedAccounts = GetAcounts();
            serializedAccounts.AddRange(accounts);

            string serializedUsers = JsonConvert.SerializeObject(accounts);
            Write(serializedUsers);

        }

        public static List<Account> GetAcounts()
        {
            string serializedUsers = Read();
            List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(serializedUsers);
            if (accounts is null)
                return Enumerable.Empty<Account>().ToList();
            return accounts;
        }

        public static void DeleteFromDB(string login)
        {
            var accounts = GetAcounts();
            accounts.Remove(accounts.FirstOrDefault(acc => acc.Login.Equals(login)));

            string serializedUsers = JsonConvert.SerializeObject(accounts);
            Write(serializedUsers);
        }
        public static void DeleteFromDB(IEnumerable<Account> deleteAccounts)
        {
            var accounts = GetAcounts();
            accounts.RemoveAll(acc => deleteAccounts.Contains(acc));

            string serializedUsers = JsonConvert.SerializeObject(accounts);
            Write(serializedUsers);
        }

        public static void DeleteFromDB(IEnumerable<string> logins)
        {
            var accounts = GetAcounts();
            accounts.RemoveAll(acc => logins.Contains(acc.Login));
            WriteToDB(accounts);
        }
        private static void Write(string text)
        {
            FileSafeAccess.WriteToFile(DBPath, text);
        }
        private static string Read()
        {
            var str = FileSafeAccess.ReadFile(DBPath);
            if (str is null)
                return "";
            return str;
        }
    }

}
