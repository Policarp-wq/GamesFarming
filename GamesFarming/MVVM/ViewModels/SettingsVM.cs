using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace GamesFarming.MVVM.ViewModels
{
    internal class SettingsVM : ViewModelBase
    {
        private string _steamPath;

        public string SteamPath
        {
            get { return _steamPath; }
            set 
            {
                _steamPath = value;
                OnPropertyChanged();
            }
        }

        private string _maFiles;

        public string MaFiles
        {
            get { return _maFiles; }
            set 
            {
                _maFiles = value;
                OnPropertyChanged();

            }
        }

        private string _steamLaunch;

        public string SteamLaunch
        {
            get { return _steamLaunch; }
            set 
            {
                _steamLaunch = value;
                OnPropertyChanged();

            }
        }

        public ICommand OnSteamExeSelector { get; set; }
        public ICommand OnMAFilesSelector { get; set; }
        public ICommand OnSubmit { get; set; }

        public SettingsVM()
        {
            SteamPath = UserSettings.ContainsSteamPath ? UserSettings.GetSteamPath() : "Select steam.exe path";
            MaFiles = UserSettings.ContainsMAFilesPath ? UserSettings.GetMAFilesPath() : "Select MA Files folder path";
            SteamLaunch = UserSettings.GetLaunchSeconds().ToString();
            OnSteamExeSelector = new RelayCommand(() => ShowSteamExeSelector());
            OnMAFilesSelector = new RelayCommand(() => ShowMAFilesSelector());
            OnSubmit = new RelayCommand(() => Submit());

        }

        public void ShowSteamExeSelector()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select steam.exe";
            openFileDialog.ShowDialog();
            SteamPath = openFileDialog.FileName;
        }
        public void ShowMAFilesSelector()
        {
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            MaFiles = folderBrowser.SelectedPath;
        }

        public void Submit()
        {
            if(!int.TryParse(SteamLaunch, out int parsed))
            {
                MessageBox.Show("Wrong steam launch seconds value : " + SteamLaunch);
                return;
            }
            UserSettings.SetLaunchSeconds(parsed);
            UserSettings.SetSteamPath(SteamPath);
            UserSettings.SetMAFilesPath(MaFiles);
            MessageBox.Show("Success!");
                
        }

    }
}
