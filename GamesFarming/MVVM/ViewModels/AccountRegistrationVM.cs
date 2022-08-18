using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class AccountRegistrationVM :ViewModelBase
    {
        private readonly ApplicationContext _context;


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
        public AccountRegistrationVM(ApplicationContext context)
        {
            _context = context;
            Register = new RelayCommand(() => RegisterAccount());
        }

        public void RegisterAccount()
        {
            Account account = new Account(Login, Password, 440, Optimize? 1: 0, 300, 300);
            //try
            //{
                _context.AddAccount(account);
                //MessageBox.Show("Congrats!");
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
    }


}
