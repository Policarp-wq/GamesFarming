using GamesFarming.MVVM.ViewModels;
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
            MainWindow = new MainWindow() { DataContext = new AccountRegistrationVM() };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
