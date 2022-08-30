using GamesFarming.DataBase;
using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Stores;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class AccountRegistrationVM: ViewModelBase
    {

        private string _login;

        public string Login
        {
            get { return _login; }
            set {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set 
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _gameCode;

        public string GameCode
        {
            get { return _gameCode; }
            set 
            {
                _gameCode = value;
                OnPropertyChanged();
            }
        }


        private string _resX;

        public string ResX
        {
            get { return _resX; }
            set 
            {
                _resX = value;
                OnPropertyChanged();
            }
        }

        private string _resY;

        public string ResY
        {
            get { return _resY; }
            set 
            {
                _resY = value;
                OnPropertyChanged();
            }
        }

        private string _configName;

        public string ConfigName
        {
            get { return _configName; }
            set 
            {
                _configName = value;
                OnPropertyChanged();
            }
        }


        private bool _optimize = true;

        public bool Optimize
        {
            get { return _optimize; }
            set 
            {
                _optimize = value;
                OnPropertyChanged();
            }
        }


        public ICommand Register { get; set; }
        public AccountRegistrationVM()
        {
            Register = new RelayCommand(() => ThreadHandler.StartInThread(RegisterAccount));
        }

        public void RegisterAccount()
        {
            try
            {
                Account account = new Account(Login, Password, int.Parse(GameCode), Optimize? 1: 0, int.Parse(ResX), int.Parse(ResY));
                JsonDB.WriteToDB(account);
                MessageBox.Show("Succesful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add the account to DB! :" +  ex.Message);
            }
        }

    }


}
