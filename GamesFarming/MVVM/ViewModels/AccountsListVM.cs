using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class AccountsListVM : ViewModelBase
    {
        private NavigationStore _navigationStore;
        private ApplicationContext _context;
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
        public AccountsListVM(NavigationStore navigationStore, ApplicationContext context)
        {
            _navigationStore = navigationStore;
            _context = context;
            Accounts = new ObservableCollection<AccountPresentation>( _context.Accounts.ToList().Select(x => new AccountPresentation(x))); // in thread
            Start = new RelayCommand(() => OnStart());
        }

        public void OnStart()
        {
            FarmingStarter.StartFarming(@"H:\Program Files (x86)\Steam\steam.exe",
                Accounts.Where(x => x.Selected)
                .Select(x => x.Account));
        }
    }
}
