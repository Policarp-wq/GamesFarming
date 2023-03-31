using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Models.Accounts;
using GamesFarming.MVVM.Models.PC;
using GamesFarming.MVVM.Models.Steam;
using GamesFarming.MVVM.Stores;
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

        public ObservableCollection<AccountPresentation> Accounts => FilterableAccounts.GetFiltered(new AccountPresentationComparer());
        private AccountPresentation _selectedItem;
        public NavigationStore NavigationStore;

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
        public FarmProgress FarmingProgress { get; set; }

        public int Minimum = 0;
        public int Maximum => FarmingProgress.AccountsCnt;
        public int Value => FarmingProgress.Current;
        public string Percents => FarmingProgress.Percents.ToString() + "%";

        public List<Account> SelectedAccounts;
        public string SelectedAccountsCnt => SelectedAccounts.Count().ToString();

        public ICommand Start { get; set; }
        public ICommand Delete { get; set; }
        public ICommand GetInfo { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand ClearCloud { get; set; }
        public ICommand SelectAll { get; set; }

        public readonly DBAccess<Account> AccountsDB;
        

        public AccountsListVM(NavigationStore navigationStore)
        {
            NavigationStore = navigationStore;
            AccountsDB = new DBAccess<Account>(DBKeys.AccountKey);
            FilterableAccounts = new FilterableCollection<AccountPresentation>(AccountsDB.GetItems().Select(x => new AccountPresentation(x))); // in thread
            SubscribeSelectionChanged(FilterableAccounts.Items);
            FilterableAccounts.FilterChanged += () => OnPropertyChanged(nameof(Accounts));
            SelectedAccounts = new List<Account>();
            FarmingProgress = new FarmProgress();
            FarmingProgress.Updated += OnFarmingProgressUpdated;
             _manager = new FarmingManager(UserSettings.GetSteamPath(), FarmingProgress);

            Start = new RelayCommand(() => OnStart());
            Delete = new RelayCommand(() => DeleteAccounts());
            Cancel = new RelayCommand(() => OnCancel());
            ClearCloud = new RelayCommand(() => OnClearCloud()); 
            GetInfo = new ParamCommand(p => OnGetInfo(p));
            SelectAll = new RelayCommand(() => OnSelectAll());
            _cancellationTokenSource = new CancellationTokenSource();
            Accounts.CollectionChanged += OnAccountsChanged;
        }

        private void OnFarmingProgressUpdated()
        {
            OnPropertyChanged(nameof(Maximum));
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(Minimum));
            OnPropertyChanged(nameof(Percents));
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

        public void OnSelectAll()
        {
            int selectdID = -1;
            if (SelectedAccounts.Count > 0)
                selectdID = SelectedAccounts.First().GameCode;
            for(int i = 0; i < Accounts.Count; ++i)
            {
                if (selectdID == -1 || Accounts[i].GameCode == selectdID)
                    Accounts[i].Selected = true;
            }
        }

        
        public void OnStart()
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                var selectedArgs = new List<LaunchArgument>(SelectedAccounts.Select(acc => new LaunchArgument(acc)));
                NavigationStore.TrayIcon.ShowBalloonTip(2000, "Start", $"Farming has started with {selectedArgs.Count} accounts",
                    System.Windows.Forms.ToolTipIcon.Info);
               _manager.StartFarming(selectedArgs, _cancellationTokenSource.Token, () =>
               {
                   UpdateLaunchTime(selectedArgs.Select(x => x.Account));
                   Update();
                   NavigationStore.TrayIcon.ShowBalloonTip(3000, "Success", "Selected accounts has been farmed", System.Windows.Forms.ToolTipIcon.Info);
               });

                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void OnCancel()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _manager.CloseFarmApps();
            }
            catch(ObjectDisposedException)
            {
                MessageBox.Show("Cancel has already been pressed. Just wait or close the panel");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void OnClearCloud()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var selectedArgs = SelectedAccounts.Select(acc => new LaunchArgument(acc));
            _manager.ClearCloudErrors(selectedArgs, _cancellationTokenSource.Token);
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

        public void UpdateLaunchTime(IEnumerable<Account> usedAccs)
        {
            if(_cancellationTokenSource.IsCancellationRequested)
                return;
            List<Account> updatedAccs = new List<Account>();
            foreach(var acc in FilterableAccounts.Items.Select(item => item.Account))
            {
                Account newAccount = acc;
                var equalAcc = usedAccs.Where(x => x.Equals(acc));
                if (equalAcc.Any())
                {
                    newAccount = equalAcc.First();
                }
                updatedAccs.Add(newAccount);
            }
            SelectedAccounts.Clear();
            AccountsDB.ReuploadDB(updatedAccs);
            
        }

        public void DeleteAccounts()
        {
            if (MessageBox.Show("Want to delete accounts?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            AccountsDB.DeleteFromDB(SelectedAccounts);
            SelectedAccounts.Clear();
            FilterableAccounts.Items = new ObservableCollection<AccountPresentation>(AccountsDB.GetItems().Select(x => new AccountPresentation(x)));
            Update();
        }

        public void Update()
        {
            FilterableAccounts.Items = new ObservableCollection<AccountPresentation>(AccountsDB.GetItems().Select(x => new AccountPresentation(x)));
            SubscribeSelectionChanged(FilterableAccounts.Items);
            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(SelectedAccounts));
            OnPropertyChanged(nameof(SelectedAccountsCnt));
        }
    }
}
