using GamesFarming.MVVM.Base;
using System;
using System.Windows;
using System.Windows.Forms;

namespace GamesFarming.MVVM.Stores
{
    internal class NavigationStore
    {
        private ViewModelBase _currentVM;
        public NotifyIcon TrayIcon;
        public Window MainWindow;
        public event Action CurrentVMChanged;
        public ViewModelBase CurrentVM
        {
            get { return _currentVM; }
            set 
            {
                _currentVM = value;
                OnCurrentVMChanged();
            }
        }

        public void OnCurrentVMChanged()
        {
            CurrentVMChanged?.Invoke();
        }

    }
}
