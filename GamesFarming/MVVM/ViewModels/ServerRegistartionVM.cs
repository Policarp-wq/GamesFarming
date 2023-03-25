using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models.Server;
using System.Windows;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class ServerRegistartionVM : ViewModelBase
    {
        private string _ip;

        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _capacity;

        public string Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }

        public ICommand RegisterServer { get; set; }
        public ServerRegistartionVM()
        {
            RegisterServer = new RelayCommand(() => OnRegisterServer());
        }

        public void OnRegisterServer()
        {
            if(!int.TryParse(Capacity, out int capacity))
            {
                MessageBox.Show("Capacity must be a number! : " + Capacity);
            }
            else
            {
                var server = new ServerInfo(Name, capacity, new Models.Connection(IP, Password));
                ServerManager.AddServer(server);
                MessageBox.Show("Successful!");
            }

        }
    }
}
