using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Stores;

namespace GamesFarming.MVVM.ViewModels
{
    internal class MainWindowVM: ViewModelBase
    {
        private NavigationStore _navigationStore;

        public ViewModelBase CurrentVM => _navigationStore.CurrentVM;

        public MainWindowVM(NavigationStore navigationStore, ApplicationContext context)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentVMChanged += OnCurrentVMChanged;
        }

        private void OnCurrentVMChanged()
        {
            OnPropertyChanged(nameof(CurrentVM));
        }
    }
}
