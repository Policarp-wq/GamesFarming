using GamesFarming.MVVM.Stores;
using GamesFarming.MVVM.ViewModels;
using GamesFarming.MVVM.Views;
using GamesFarming.User;
using System;
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
                NavigationStore navigationStore = new NavigationStore
                {
                    CurrentVM = new AccountsListVM()
                };
                MainWindow = new MainWindow() { DataContext = new MainWindowVM(navigationStore) };
                MainWindow.Show();
                if (!(UserSettings.ContainsSteamPath && UserSettings.ContainsMAFilesPath))
                {
                    SettingsView settings = new SettingsView() { DataContext = new SettingsVM() };
                    settings.Show();
                }
                base.OnStartup(e);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка запуска приложения! : " + ex.Message);
                throw ex;
            }
        }
    }
}
