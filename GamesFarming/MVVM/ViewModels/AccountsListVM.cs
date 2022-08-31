using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.User;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class AccountsListVM : ViewModelBase
    {
        private ObservableCollection<AccountPresentation> _accounts;

        public ObservableCollection<AccountPresentation> Accounts
        {
            get { return _accounts; }
            set 
            {
                _accounts = value;
                OnPropertyChanged();
            }
        }

        private AccountPresentation _selectedItem;

        public AccountPresentation SelectedItem
        {
            get { return _selectedItem; }
            set 
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand Start { get; set; }
        public ICommand Delete { get; set; }
        public AccountsListVM()
        {
            Accounts = new ObservableCollection<AccountPresentation>(JsonDB.GetAcounts().Select(x => new AccountPresentation(x))); // in thread
            Start = new RelayCommand(() => OnStart());
            Delete = new RelayCommand(() => DeleteAccounts());
        }

        public IEnumerable<Account> SelectedAccounts => Accounts.Where(x => x.Selected)
                                                                     .Select(x => x.Account);

        public void OnStart()
        {
            FarmingStarter.StartFarming(UserSettings.GetSteamPath(),
                SelectedAccounts);
        }

        public void DeleteAccounts()
        {
            JsonDB.DeleteFromDB(SelectedAccounts);
            Accounts = new ObservableCollection<AccountPresentation>(JsonDB.GetAcounts().Select(x => new AccountPresentation(x)));
            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(SelectedAccounts));
        }
    }
}
