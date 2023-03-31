using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Views;
using System;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class TimerVM :ViewModelBase
    {
        private readonly Timer _timer;
        public string StringTimer => TimeSpan.FromSeconds(_timer.CurrentSeconds).ToString();
        public TimerView TimerView { get; set; }
        public ICommand Close { get; set; }
        public TimerVM(Timer timer)
        {
            _timer = timer;
            _timer.TimerTicked += () => { OnPropertyChanged(nameof(StringTimer)); };
            Close = new RelayCommand(() => TimerView.WindowState = System.Windows.WindowState.Minimized);
        }
	}
}
