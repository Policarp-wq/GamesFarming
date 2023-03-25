using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models.Server;
using GamesFarming.MVVM.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class ServersListVM : ViewModelBase
    {
        public ObservableCollection<ServerPresentation> Servers { get; set; }
        public List<ServerInfo> SelectedServers;
        public ICommand AddServer { get;set; }
        public ICommand RemoveServer { get;set; }
        public ServersListVM()
        {
            Servers = new ObservableCollection<ServerPresentation>(ServerManager.GetServers().Select(serv => new ServerPresentation(serv)));
            SelectedServers = new List<ServerInfo>();
            AddServer = new RelayCommand(() => OnAddServer());
            RemoveServer = new RelayCommand(() => OnRemoveServer());
            ServerManager.ServersChanged += Update;
            Update();
        }
        
        public void SubscribeSelectionChanged(IEnumerable<ServerPresentation> servers)
        {
            foreach (var el in servers)
            {
                el.SelectedChanged += (server, selected) =>
                {
                    if (selected)
                        SelectedServers.Add(server);
                    else SelectedServers.Remove(server);
                };
            }
        }

        public void OnAddServer()
        {
            ServerRegistrationView serverRegistration = new ServerRegistrationView();
            serverRegistration.DataContext = new ServerRegistartionVM();
            serverRegistration.Show();
        }
        public void OnRemoveServer()
        {
            ServerManager.Remove(SelectedServers);
            SelectedServers.Clear();
        }
        private void Update()
        {
            Servers = new ObservableCollection<ServerPresentation>(ServerManager.GetServers().Select(serv => new ServerPresentation(serv)));
            OnPropertyChanged(nameof(Servers));
            OnPropertyChanged(nameof(SelectedServers));
            SubscribeSelectionChanged(Servers);
        }

    }
}
