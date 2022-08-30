using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Stores;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class MainWindowVM: ViewModelBase
    {
        private NavigationStore _navigationStore;

        public ViewModelBase CurrentVM => _navigationStore.CurrentVM;

        public ICommand MoveToRegistration { get; set; }
        public ICommand MoveToAccounts { get; set; }
        public MainWindowVM(NavigationStore navigationStore)
        {
            
            _navigationStore = navigationStore;
            _navigationStore.CurrentVMChanged += OnCurrentVMChanged;
            MoveToAccounts = new RelayCommand(() => _navigationStore.CurrentVM = new AccountsListVM());
            MoveToRegistration = new RelayCommand(() => _navigationStore.CurrentVM = new AccountRegistrationVM());
        }

        private void OnCurrentVMChanged()
        {
            OnPropertyChanged(nameof(CurrentVM));
        }
    }
}
