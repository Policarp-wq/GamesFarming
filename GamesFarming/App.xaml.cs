using GamesFarming.DataBase;
using GamesFarming.MVVM.Stores;
using GamesFarming.MVVM.ViewModels;
using System;
using System.Windows;

namespace GamesFarming
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ApplicationContext context = CreateAppContext();
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentVM = new AccountsListVM(navigationStore, context);
            MainWindow = new MainWindow() { DataContext = new MainWindowVM(navigationStore, context)};
            MainWindow.Show();
            base.OnStartup(e);
        }

        private ApplicationContext CreateAppContext()
        {
            try
            {
                ApplicationContext context = new ApplicationContext();
                return context;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }
    }
}
