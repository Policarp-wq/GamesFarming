using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite; 
//public int ID { get; set; }
//public string Login { get; set; }
//public string Password { get; set; }
//public int GameCode { get; set; }
//public int Optimize { get; set; }
//public int ResX { get; set; }
//public int ResY { get; set; }

namespace GamesFarming.DataBase
{
    internal class DataBaseAccess
    {
        public static string LoadConnectionString(string id = "DefaultConnection")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static IEnumerable<Account> LoadAccounts()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Account>("select * from Accounts", new DynamicParameters());
                return output;
            }
        }
        public static void SaveAccount(Account account)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Accounts (Login, Password, GameCode, Optimize, ResX, ResY)" +
                    "values (@Login, @Password, @GameCode, @Optimize, @ResX, @ResY)", account);

            }
        }
    }
}
