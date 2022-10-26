using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
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

        private CancellationTokenSource _cancellationTokenSource;

        public ICommand Start { get; set; }
        public ICommand Delete { get; set; }
        public ICommand GetInfo { get; set; }
        public ICommand Cancel { get; set; }

        private readonly FarmingStarter _starter;
        public AccountsListVM()
        {
            Accounts = new ObservableCollection<AccountPresentation>(JsonDB.GetAcounts().Select(x => new AccountPresentation(x))); // in thread
            Start = new RelayCommand(() => OnStart());
            Delete = new RelayCommand(() => DeleteAccounts());
            Cancel = new RelayCommand(() => OnCancel());
            GetInfo = new ParamCommand(p => OnGetInfo(p));
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public IEnumerable<Account> SelectedAccounts => Accounts.Where(x => x.Selected)
                                                                     .Select(x => x.Account);

        public void OnStart()
        {
            try
            {
                FarmingStarter starter = new FarmingStarter(UserSettings.GetSteamPath(), SelectedAccounts);
                starter.StartFarming(_cancellationTokenSource.Token);
                UpdateLaunchTime(SelectedAccounts);
                Update();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void OnCancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public void OnGetInfo(object p)
        {
            var tuple = p as Tuple<object, object>;
            string output = "";
            if (tuple is null)
                output = "Wrong convertation : " + p.ToString();
            else
            {
                string login = tuple.Item1.ToString();
                string gameCode = tuple.Item2.ToString();
                if (!int.TryParse(gameCode, out int code))
                    output = "Wrong code : " + gameCode.ToString();
                else output = Accounts.Where(acc => acc.Login == login && acc.GameCode == code)
                                        .Select(acc => acc.Account)
                                        .FirstOrDefault()
                                        .ToString();
            }
            MessageBox.Show(output);

        }

        public void UpdateLaunchTime(IEnumerable<Account> changedAccounts)
        {
            List<Account> updatedAccs = new List<Account>();
            foreach(var acc in Accounts.Select(x => x.Account))
            {
                Account newAccount = acc;
                if (changedAccounts.Contains(acc))
                {
                    newAccount.LastLaunchDate = System.DateTime.Now;
                }
                updatedAccs.Add(newAccount);
            }
            JsonDB.ReuploadDB(updatedAccs);
            
        }

        public void DeleteAccounts()
        {
            JsonDB.DeleteFromDB(SelectedAccounts);
            Accounts = new ObservableCollection<AccountPresentation>(JsonDB.GetAcounts().Select(x => new AccountPresentation(x)));
            Update();
        }

        public void Update()
        {
            Accounts = new ObservableCollection<AccountPresentation>(JsonDB.GetAcounts().Select(x => new AccountPresentation(x)));
            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(SelectedAccounts));
        }
    }
}
