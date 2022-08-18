﻿using GamesFarming.MVVM.Base;
using System;

namespace GamesFarming.MVVM.Stores
{
    internal class NavigationStore
    {
        private ViewModelBase _currentVM;

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
