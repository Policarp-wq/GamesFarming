using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Stores;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class MainWindowVM: ViewModelBase
    {
        private NavigationStore _navigationStore;

        public ViewModelBase CurrentVM => _navigationStore.CurrentVM;

        public ICommand MoveToRegistration { get; set; }
        public ICommand MoveToAccounts { get; set; }
        public ICommand Import { get; set; }
        public MainWindowVM(NavigationStore navigationStore)
        {
            
            _navigationStore = navigationStore;
            _navigationStore.CurrentVMChanged += OnCurrentVMChanged;
            MoveToAccounts = new RelayCommand(() => _navigationStore.CurrentVM = new AccountsListVM());
            MoveToRegistration = new RelayCommand(() => _navigationStore.CurrentVM = new AccountRegistrationVM());
            Import = new RelayCommand(() => ImportFiles());
        }

        private void OnCurrentVMChanged()
        {
            OnPropertyChanged(nameof(CurrentVM));
        }

        public void ImportFiles()
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            ThreadHandler.StartInThread(() => DeserializeInDB(folderBrowser.SelectedPath));
        }

        private void DeserializeInDB(string folderPath)
        {
            try
            {
                var deserialized = BatDeserializer.DeserializeFolderFiles(folderPath, OnExceptionCaused);
                JsonDB.WriteToDB(deserialized);
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
