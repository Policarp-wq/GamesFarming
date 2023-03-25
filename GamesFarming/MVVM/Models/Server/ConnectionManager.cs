using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesFarming.MVVM.Models.Server
{
    internal class ConnectionManager
    {
        public readonly List<ServerInfo> Servers;
        private int _pos;

        public ConnectionManager()
        {
            _pos = 0;
            Servers = ServerManager.GetServers();
        }
        public Connection GetNextConnect()
        {
            if (_pos >= Servers.Count)
                return null;
            if (!Servers[_pos].CanConnect)
            {
                _pos++;
                if (_pos >= Servers.Count)
                    return null;
            }
            Servers[_pos].Connect();
            return Servers[_pos].Connection;
        }
    }
}
