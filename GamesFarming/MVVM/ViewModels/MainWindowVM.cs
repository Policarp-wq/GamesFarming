using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Stores;
using GamesFarming.MVVM.Views;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class MainWindowVM: ViewModelBase
    {
        private NavigationStore _navigationStore;
        public const string HelpMessage = "If you have any questions or you have found a bug, contact me tg:@Policarp228";

        public ViewModelBase CurrentVM => _navigationStore.CurrentVM;

        public ICommand MoveToRegistration { get; set; }
        public ICommand MoveToAccounts { get; set; }
        public ICommand MoveToServers { get; set; }
        public ICommand Import { get; set; }
        public ICommand OpenSettings { get; set; }
        public ICommand GetHelp { get; set; }
        public ICommand Close { get; set; }
        public ICommand DragWindow { get; set; }
        public readonly DBAccess<Account> AccountsDB;

        public MainWindowVM(NavigationStore navigationStore)
        {
            AccountsDB = new DBAccess<Account>(DBKeys.AccountKey);
            _navigationStore = navigationStore;
            _navigationStore.CurrentVMChanged += OnCurrentVMChanged;

            var accountsVM = new AccountsListVM(navigationStore);
            var serversVM = new ServersListVM();
            var registerVM = new AccountRegistrationVM();

            _navigationStore.CurrentVM = accountsVM;
            DragWindow = new RelayCommand(() => _navigationStore.MainWindow.DragMove());
            Close = new RelayCommand(() => _navigationStore.MainWindow.Hide());
            MoveToAccounts = new RelayCommand(() => _navigationStore.CurrentVM = accountsVM);
            MoveToServers = new RelayCommand(() => _navigationStore.CurrentVM = serversVM);
            MoveToRegistration = new RelayCommand(() => _navigationStore.CurrentVM = registerVM);
            Import = new RelayCommand(() => ImportFiles());
            OpenSettings = new RelayCommand(() => OnOpenSettings());
            GetHelp = new RelayCommand(() => MessageBox.Show(HelpMessage, "Info", MessageBoxButtons.OK));
        }

        private void OnCurrentVMChanged()
        {
            OnPropertyChanged(nameof(CurrentVM));
        }

        public void ImportFiles()
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            DeserializeInDB(folderBrowser.SelectedPath);
        }

        public void OnOpenSettings()
        {
            SettingsView settings = new SettingsView() { DataContext = new SettingsVM() };
            settings.Show();
        }

        private void DeserializeInDB(string folderPath)
        {
            try
            {
                var deserialized = BatDeserializer.DeserializeFolderFiles(folderPath, OnExceptionCaused);
                AccountsDB.WriteToDB(deserialized);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool OnExceptionCaused(string filePath, Exception exception)
        {
            var result = MessageBox.Show(exception.Message + "Continue import? File path : " + filePath, "Error", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                return true;
            return false;
        }

        
    }
}
