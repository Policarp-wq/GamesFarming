using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Models.Accounts;
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

        private CancellationTokenSource _cancellationTokenSource;
        private readonly FarmingManager _manager;
        public FilterableCollection<AccountPresentation> FilterableAccounts;

        public ObservableCollection<AccountPresentation> Accounts => FilterableAccounts.GetFiltered();
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

        private string _filterString;

        public string FilterString
        {
            get { return _filterString; }
            set 
            {
                _filterString = value;
                OnPropertyChanged();
                SetFilter();
            }
        }
        public List<Account> SelectedAccounts;
        public string SelectedAccountsCnt => SelectedAccounts.Count().ToString();

        public ICommand Start { get; set; }
        public ICommand Delete { get; set; }
        public ICommand GetInfo { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand ClearCloud { get; set; }

        public AccountsListVM()
        {
            FilterableAccounts = new FilterableCollection<AccountPresentation>(JsonDB.GetAcounts().Select(x => new AccountPresentation(x))); // in thread
            SubscribeSelectionChanged(FilterableAccounts.Items);
            FilterableAccounts.FilterChanged += () => OnPropertyChanged(nameof(Accounts));
            SelectedAccounts = new List<Account>();
            Start = new RelayCommand(() => OnStart());
            Delete = new RelayCommand(() => DeleteAccounts());
            Cancel = new RelayCommand(() => OnCancel());
            ClearCloud = new RelayCommand(() => OnClearCloud()); 
            GetInfo = new ParamCommand(p => OnGetInfo(p));
            _cancellationTokenSource = new CancellationTokenSource();
             _manager = new FarmingManager(UserSettings.GetSteamPath());
            Accounts.CollectionChanged += OnAccountsChanged;
        }

        private void OnAccountsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
             OnPropertyChanged(nameof(SelectedAccountsCnt));
        }

        public void SubscribeSelectionChanged(IEnumerable<AccountPresentation> accounts)
        {
            foreach (var el in accounts)
            {
                el.SelectedChanged += (acc, selected) =>
                {
                    if (selected)
                        SelectedAccounts.Add(acc);
                    else SelectedAccounts.Remove(acc);
                    OnPropertyChanged(nameof(SelectedAccountsCnt));
                };
            }
        }

        
        public void OnStart()
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _manager.StartFarming(SelectedAccounts.Select(acc => new LaunchArgument(acc)), _cancellationTokenSource.Token);
                UpdateLaunchTime();
                Update();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void OnCancel()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
        public void OnClearCloud()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _manager.ClearCloudErrors(SelectedAccounts.Select(acc => new LaunchArgument(acc)), _cancellationTokenSource.Token);
        }

        public void SetFilter()
        {
            var general = Filters.GetGeneralFilter(FilterString);
            if (general is null)
                FilterableAccounts.ClearFilter();
            else FilterableAccounts.SetFilter(x => general(x.Account));
        }


        public void OnGetInfo(object p)
        {
            Tuple<object, object> tuple = p as Tuple<object, object>;
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

        public void UpdateLaunchTime()
        {
            List<Account> updatedAccs = new List<Account>();
            foreach(var acc in FilterableAccounts.Items.Select(item => item.Account))
            {
                Account newAccount = acc;
                if (SelectedAccounts.Contains(acc))
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
            FilterableAccounts.Items = new ObservableCollection<AccountPresentation>(JsonDB.GetAcounts().Select(x => new AccountPresentation(x)));
            Update();
        }

        public void Update()
        {
            FilterableAccounts.Items = new ObservableCollection<AccountPresentation>(JsonDB.GetAcounts().Select(x => new AccountPresentation(x)));
            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(SelectedAccounts));
        }
    }
}
