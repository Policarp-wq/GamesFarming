using GamesFarming.DataBase;
using System;
using System.Collections.Generic;

namespace GamesFarming.MVVM.Models.Server
{
    public static class ServerManager
    {
        public static DBAccess<ServerInfo> ServersDB = new DBAccess<ServerInfo>(DBKeys.ServerKey);
        public static event Action ServersChanged;
        public static int ServersAmount => GetServers().Count;
        static ServerManager()
        {
            ServersDB.DBChanged += OnServersChanged;
        }

        private static void OnServersChanged()
        {
            ServersChanged?.Invoke();
        }

        public static void AddServer(ServerInfo server)
        {
            ServersDB.WriteToDB(server);
        }
        public static void Remove(ServerInfo server)
        {
            ServersDB.DeleteFromDB(server);
        }
        public static void Remove(IEnumerable<ServerInfo> servers)
        {
            ServersDB.DeleteFromDB(servers);
        }
        public static List<ServerInfo> GetServers()
        {
            return ServersDB.GetItems();
        }
    }
}
