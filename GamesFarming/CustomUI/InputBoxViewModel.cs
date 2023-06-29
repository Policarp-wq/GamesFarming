using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using System;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class InputBoxViewModel : ViewModelBase
    {
        private string _inputBoxText;

        public string InputBoxText
        {
            get { return _inputBoxText; }
            set 
            {
                _inputBoxText = value;
                OnPropertyChanged();
            }
        }
        private string _input;

        public string Input
        {
            get { return _input; }
            set 
            {
                _input = value;
                OnPropertyChanged();
            }
        }

        public ICommand Confirm { get; set; }
        public InputBoxViewModel(string inputBoxText, Action<string> actionWithInput)
        {
            InputBoxText = inputBoxText;
            Confirm = new RelayCommand(() => actionWithInput?.Invoke(Input));
        }


    }
}
