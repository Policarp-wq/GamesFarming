using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using System.Windows;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class AccountRegistrationVM :ViewModelBase
    {
        private string _login;

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private bool _optimize = true;

        public bool Optimize
        {
            get { return _optimize; }
            set { _optimize = value; }
        }

        private bool _windowed = true;

        public bool Windowed
        {
            get { return _windowed; }
            set { _windowed = value; }
        }

        public ICommand Register { get; set; }
        public AccountRegistrationVM()
        {
            Register = new RelayCommand(() => RegisterAccount());
        }

        public void RegisterAccount()
        {
            MessageBox.Show("Congrats!");
        }
    }


}
