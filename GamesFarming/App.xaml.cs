using GamesFarming.DataBase;
using GamesFarming.MVVM.Models;
using GamesFarming.MVVM.Stores;
using GamesFarming.MVVM.ViewModels;
using GamesFarming.User;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace GamesFarming
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string UserFileName = "UserData.txt";
        public static string UserDataDirectory = @".";
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (!UserSettings.ContainsSteamPath)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Title = "Select steam.exe";
                    openFileDialog.ShowDialog();
                    UserSettings.SetSteamPath(openFileDialog.FileName);
                }
                NavigationStore navigationStore = new NavigationStore();
                navigationStore.CurrentVM = new AccountsListVM();
                MainWindow = new MainWindow() { DataContext = new MainWindowVM(navigationStore) };
                MainWindow.Show();
                base.OnStartup(e);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка запуска приложения! : " + ex.Message + ex.InnerException.Message);
                throw ex;
            }
        }
    }
}
