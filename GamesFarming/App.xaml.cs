using GamesFarming.MVVM.Models.PC;
using GamesFarming.MVVM.Stores;
using GamesFarming.MVVM.ViewModels;
using GamesFarming.MVVM.Views;
using GamesFarming.User;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Froms = System.Windows.Forms;

namespace GamesFarming
{
    public partial class App : Application
    {
        private Froms.NotifyIcon _trayIcon;

        public static string UserFileName = "UserData.txt";
        public static string IconPath = "Resourses/Icons/icon3.ico";
        public static string UserDataDirectory = @".";

        private MainWindow _mainWindow;
        private NavigationStore _navigationStore;

        protected override void OnStartup(StartupEventArgs e)
        {
            CheckIsAlreadyRunning();
            try
            {
                _trayIcon = GetIcon();
                _navigationStore = new NavigationStore
                {
                    TrayIcon = _trayIcon,
                };
                //_navigationStore.CurrentVM = new AccountsListVM(_navigationStore);
                _mainWindow = new MainWindow() { DataContext = new MainWindowVM(_navigationStore) };
                _navigationStore.MainWindow = _mainWindow;
                _mainWindow.Show();
                if (!(UserSettings.ContainsSteamPath && UserSettings.ContainsMAFilesPath))
                {
                    SettingsView settings = new SettingsView() { DataContext = new SettingsVM() };
                    settings.Show();
                }
                base.OnStartup(e);
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка запуска приложения! : " + ex.Message);
                throw ex;
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.InnerException.Message,
                "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void CheckIsAlreadyRunning()
        {
            var appName = Process.GetCurrentProcess().ProcessName;
            if(TaskManager.GetProcesses(appName).Count() > 1)
            {
                MessageBox.Show("Another instance of the app is already running",
                    "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                Current.Shutdown();
            }
                
        }

        private Froms.NotifyIcon GetIcon()
        {
            var icon = new Froms.NotifyIcon
            {
                Icon = new System.Drawing.Icon(IconPath),
                Text = "Farming panel",
                Visible = true
            };
            icon.DoubleClick += OnTreyIconDoubleClicked;
            icon.ContextMenu = new Froms.ContextMenu();
            icon.ContextMenu.MenuItems.Add(new Froms.MenuItem("Exit", (e, a) => Current.Shutdown()));
            return icon;
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _trayIcon.Dispose();
            base.OnExit(e);
        }

        private void OnTreyIconDoubleClicked(object sender, EventArgs e)
        {
            _mainWindow.Show();
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Activate();
        }
    }
}
