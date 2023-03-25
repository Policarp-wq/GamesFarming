using System.Collections.Generic;

namespace GamesFarming.DataBase
{
    public class DBKeys
    {
        private static string _accountsCode = "effac102";
        private static string _serversCode = "aueur671";

        private string _code;

        public static DBKeys AccountKey = new DBKeys(_accountsCode);
        public static DBKeys ServerKey = new DBKeys(_serversCode);
        public static Dictionary<DBKeys, string> DBFileNames = new Dictionary<DBKeys, string>();
        static DBKeys()
        {
            DBFileNames.Add(AccountKey, "UsersDB");
            DBFileNames.Add(ServerKey, "ServersDB");
        }
        private DBKeys(string code)
        {
            _code = code;
        }
        public override int GetHashCode()
        {
            return _code.GetHashCode();
        }
    }
}
