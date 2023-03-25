using System;

namespace GamesFarming.MVVM.Models.Server
{
    public class ServerPresentation
    {
        public ServerInfo Server { get; set; }
        public string Name => Server.Name;
        public string IP => Server.Connection.IP;
        public string Capacity => Server.Capacity.ToString();
        public Action<ServerInfo, bool> SelectedChanged;
        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set 
            {
                _selected = value;
                SelectedChanged?.Invoke(Server, value);
            }
        }
        public ServerPresentation(ServerInfo server)
        {
            Server = server;
            Selected = false;
        }
    }
}
