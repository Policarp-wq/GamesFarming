using System;

namespace GamesFarming.MVVM.Models.Server
{
    public class ServerInfo
    {
        public string Name { get; set; }
        public int Capacity { get; private set; }
        public int CurrentPlayers { get; private set; }
        public bool IsFull => CurrentPlayers >= Capacity;
        public bool CanConnect => !IsFull;
        public Connection Connection { get; set; }
        public ServerInfo(string name, int capacity, Connection connection)
        {
            Name = name;
            Capacity = capacity;
            if (Capacity < 0)
                throw new ArgumentOutOfRangeException("Capacity can't be less than 0 : " + Capacity.ToString());
            Connection = connection;
        }
        public void Connect()
        {
            if (IsFull)
                throw new Exception("Server is full!");
            CurrentPlayers++;
        }
        public void Disconnect()
        {
            if(CurrentPlayers > 0)
                CurrentPlayers--;
        }
        public override bool Equals(object obj)
        {
            var server = obj as ServerInfo;
            if (server == null)
                return false;
            return GetHashCode() == server.GetHashCode();
        }
        public override int GetHashCode()
        {
            return Connection.GetHashCode();
        }
    }
}
