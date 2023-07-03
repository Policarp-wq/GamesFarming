using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Models.Accounts;
using GamesFarming.MVVM.Models.ASF;
using GamesFarming.MVVM.Models.Steam;
using GamesFarming.MVVM.Stores;
using GamesFarming.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class AccountsListVM : ViewModelBase
    {
        public const string CommandsFile = "Commands.txt";
        public CancellationTokenSource CancellationSource;
        public readonly FarmingManager FarmingManager;
        public FilterableCollection<AccountPresentation> FilterableAccounts;

        public ObservableCollection<AccountPresentation> Accounts => FilterableAccounts.GetFiltered(new AccountPresentationComparer());

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
        public string StringTimer => TimeSpan.FromSeconds(FarmingManager.QueueTimer.CurrentSeconds).ToString();

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
        public ICommand CompleteFarming { get; set; }
        public ICommand ClearCloud { get; set; }
        public ICommand SelectAll { get; set; }

        public readonly DBAccess<Account> AccountsDB;

        public AccountsListVM(NavigationStore navigationStore)
        {
            NavigationStore = navigationStore;
            AccountsDB = new DBAccess<Account>(DBKeys.AccountKey);
            FilterableAccounts = new FilterableCollection<AccountPresentation>(AccountsDB.GetItems().Select(x => new AccountPresentation(x)));
            SubscribeSelectionChanged(FilterableAccounts.Items);
            FilterableAccounts.FilterChanged += () => OnPropertyChanged(nameof(Accounts));
            SelectedAccounts = new List<Account>();
            FarmingProgress = new FarmProgress();
            FarmingProgress.Updated += OnFarmingProgressUpdated;
            CancellationSource = new CancellationTokenSource();
            FarmingManager = new FarmingManager(UserSettings.GetSteamPath(), FarmingProgress);
            FarmingManager.QueueTimer.TimerTicked += () => OnPropertyChanged(nameof(StringTimer));

            Start = new RelayCommand(() => OnStart());
            Delete = new RelayCommand(() => DeleteAccounts());
            Cancel = new RelayCommand(() => OnCancel());
            CompleteFarming = new RelayCommand(() => OnCompleteFarming());
            ClearCloud = new RelayCommand(() => OnClearCloud()); 
            GetInfo = new ParamCommand(p => OnGetInfo(p));
            SelectAll = new RelayCommand(() => OnSelectAll());
            Accounts.CollectionChanged += OnAccountsChanged;

            StartFarmReadyCheck();
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

        public async void StartFarmReadyCheck()
        {
            //const int hoursTick = 1;
            //await Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        Check();
            //        Thread.Sleep(hoursTick * 3600 * 1000);
            //    }
            //});
        }

        public void Check()
        {
            int cnt = 0;
            foreach(var acc in Accounts)
            {
                if (acc.NeedToLaunch)
                    ++cnt;
            }
            if (cnt > 0)
                NavigationStore.TrayIcon.ShowBalloonTip(2, "Attention!", $"You have {cnt} accounts to farm!",
                    System.Windows.Forms.ToolTipIcon.Info);
        }

        public async void OnSelectAll()
        {
            int selectdID = -1;
            if (SelectedAccounts.Count > 0)
                selectdID = SelectedAccounts.First().GameCode;
            await Task.Run(() =>
            {
                for (int i = 0; i < Accounts.Count; ++i)
                {
                    if (selectdID == -1 || Accounts[i].GameCode == selectdID)
                        Accounts[i].Selected = true;
                }
            });
        }
        public async void OnStart()
        {
            try
            {
                CancellationSource = new CancellationTokenSource();
                var selectedArgs = new List<LaunchArgument>(SelectedAccounts.Select(acc => new LaunchArgument(acc)));
                if (selectedArgs.Count == 0)
                    return;
                var farmTime = await FarmingManager.StartFarming(selectedArgs, CancellationSource, () =>
                {
                    if (!CancellationSource.IsCancellationRequested)
                    {
                        UpdateLaunchTime(selectedArgs.Select(x => x.Account));
                        NavigationStore.TrayIcon.ShowBalloonTip(3, "Success", "Selected accounts has been farmed", System.Windows.Forms.ToolTipIcon.Info);
                        SaveLootCommand(ASFCommands.LootAccounts(selectedArgs.Select(acc => acc.Account.Login).ToList(),
                       selectedArgs.First().Account.GameCode));
                    }
                });
                NavigationStore.TrayIcon.ShowBalloonTip(5, "Start", $"Farming has started with {selectedArgs.Count} accounts for {farmTime}",
                                    System.Windows.Forms.ToolTipIcon.Info);
            }
            catch (AggregateException ex)
            {
                foreach (Exception e in ex.InnerExceptions)
                {
                    if (e is OperationCanceledException)
                        return;
                }
                throw ex;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caused on farming: " + ex.InnerException.Message);
            }
            finally
            {
                Update();
            }
        }

        private void OnCompleteFarming()
        {
            FarmingManager.CompleteEraly();
        }
        public void OnCancel()
        {
            try
            {
                CancellationSource?.Cancel();
                CancellationSource?.Dispose();
                FarmingManager.CloseFarmApps();
                SelectedAccounts.Clear();
                Update();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveLootCommand(string command)
        {
            try
            {
                FileSafeAccess.AddLineToFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), CommandsFile),
                                command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while saving commands:" + ex.Message);
            }
            
        }

        public void OnClearCloud()
        {
            CancellationSource = new CancellationTokenSource();
            var selectedArgs = SelectedAccounts.Select(acc => new LaunchArgument(acc));
            FarmingManager.ClearCloudErrors(selectedArgs, CancellationSource.Token);
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
            Account account = null;
            if (tuple is null)
                output = "Wrong convertation : " + p.ToString();
            else
            {
                string login = tuple.Item1.ToString();
                string gameCode = tuple.Item2.ToString();
                if (!int.TryParse(gameCode, out int code))
                    output = "Wrong code : " + gameCode.ToString();
                else account = Accounts.Where(acc => acc.Login == login && acc.GameCode == code)
                                        .Select(acc => acc.Account)
                                        .FirstOrDefault();
            }
            output = account.ToString() + "\n\nLaunch argument is: {" + (new LaunchArgument(account)).ToString() + "}";
            MessageBox.Show(output);

        }

        public void UpdateLaunchTime(IEnumerable<Account> usedAccs)
        {
            if(CancellationSource.IsCancellationRequested)
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
