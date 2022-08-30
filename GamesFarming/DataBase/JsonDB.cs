using GamesFarming.MVVM.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesFarming.DataBase
{
    public static class JsonDB
    {
        public static string UsersFileName = "UsersDB";
        public static string FolderPath;
        public static string DBPath => FolderPath + UsersFileName;
        static JsonDB()
        {
            FolderPath = Path.GetTempPath();
            ThreadHandler.StartInThread(CheckUsersDB);
        }

        private static void CheckUsersDB()
        {
            if(!File.Exists(DBPath))
            {
                File.CreateText(DBPath);
            }
        }

        public static void WriteToDB(Account account)
        {
            var serializedAccounts = GetAcounts();
            serializedAccounts.Add(account);

            string serializedUsers = JsonConvert.SerializeObject(serializedAccounts);
            File.WriteAllText(DBPath, serializedUsers);
            
        }
        public static void WriteToDB(IEnumerable<Account> accounts)
        {
            var serializedAccounts = GetAcounts();
            serializedAccounts.AddRange(accounts);

            string serializedUsers = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(serializedUsers, DBPath);
            
        }

        public static List<Account> GetAcounts()
        {
            string serializedUsers = File.ReadAllText(DBPath);
            List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(serializedUsers);
            if (accounts is null)
                return Enumerable.Empty<Account>().ToList();
            return accounts;
        }

        public static void DeleteFromDB(string login)
        {
            var accounts = GetAcounts();
            accounts.Remove(accounts.FirstOrDefault(acc => acc.Login == login));
            WriteToDB(accounts);
        }
        public static void DeleteFromDB(IEnumerable<Account> deleteAccounts)
        {
            var accounts = GetAcounts();
            accounts.RemoveAll(acc => deleteAccounts.Contains(acc));
            WriteToDB(accounts);
        }

        public static void DeleteFromDB(IEnumerable<string> logins)
        {
            var accounts = GetAcounts();
            accounts.RemoveAll(acc => logins.Contains(acc.Login));
            WriteToDB(accounts);
        }
    }

}
