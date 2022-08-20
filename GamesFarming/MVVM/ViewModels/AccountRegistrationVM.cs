using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Stores;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class AccountRegistrationVM: ViewModelBase
    {
        private readonly ApplicationContext _context;
        private readonly NavigationStore _navigationStore;
        private ProcessStarter _starter;

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

        private string _resX;

        public string ResX
        {
            get { return _resX; }
            set { _resX = value; }
        }

        private string _resY;

        public string ResY
        {
            get { return _resY; }
            set { _resY = value; }
        }

        private string _configName;

        public string ConfigName
        {
            get { return _configName; }
            set { _configName = value; }
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
        public ICommand Start { get; set; }
        public AccountRegistrationVM(NavigationStore store, ApplicationContext context)
        {
            _context = context;
            _navigationStore = store;
            _starter = new ProcessStarter(@"H:\Program Files (x86)\Steam\steam.exe");
            Register = new RelayCommand(() => ThreadHandler.StartInThread(RegisterAccount));
            Start = new RelayCommand(() => StartFarming());

        }

        public void RegisterAccount()
        {
            Account account = new Account(Login, Password, 440, Optimize? 1: 0, 300, 300);
            try
            {
                _context.AddAccount(account);
                MessageBox.Show("Succesful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void StartFarming()
        {
            try
            {
                _starter.Start(new LaunchArgument(_context.Accounts.ToList().First()));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }


}
