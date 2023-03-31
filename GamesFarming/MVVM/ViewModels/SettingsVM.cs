using GamesFarming.MVVM.Base;
using GamesFarming.MVVM.Commands;
using GamesFarming.User;
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

        private string _accsInGroup;

        public string AccsInGroup
        {
            get { return _accsInGroup; }
            set 
            {
                _accsInGroup = value;
                OnPropertyChanged();
            }
        }
        private string _farmTimeHrs;

        public string FarmTimeHrs
        {
            get { return _farmTimeHrs; }
            set 
            {
                _farmTimeHrs = value;
                OnPropertyChanged();
            }
        }

        private string _farmTimeMins;

        public string FarmTimeMins
        {
            get { return _farmTimeMins; }
            set 
            {
                _farmTimeMins = value;
                OnPropertyChanged();    
            }
        }

        private string _cfg;

        public string Cfg
        {
            get { return _cfg; }
            set 
            {
                _cfg = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnSteamExeSelector { get; set; }
        public ICommand OnMAFilesSelector { get; set; }
        public ICommand OnCfgSelector { get; set; }
        public ICommand OnSubmit { get; set; }

        public SettingsVM()
        {
            SteamPath = UserSettings.ContainsSteamPath ? UserSettings.GetSteamPath() : "Select steam.exe path";
            MaFiles = UserSettings.ContainsMAFilesPath ? UserSettings.GetMAFilesPath() : "Select MA Files folder path";
            Cfg = UserSettings.ContainsCfgPath ? UserSettings.GetCfgPath() : "Select Cfg folder path";
            SteamLaunch = UserSettings.GetLaunchSeconds().ToString();
            FarmTimeHrs = UserSettings.GetFarmTimeHours().ToString();
            FarmTimeMins = UserSettings.GetFarmTimeMinutes().ToString();
            AccsInGroup = UserSettings.GetAccsInGroup().ToString();
            OnSteamExeSelector = new RelayCommand(() => ShowSteamExeSelector());
            OnMAFilesSelector = new RelayCommand(() => ShowMAFilesSelector());
            OnCfgSelector = new RelayCommand(() => ShowCfgSelector());
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
            folderBrowser.Tag = "Select maFiles folder";
            folderBrowser.ShowDialog();
            MaFiles = folderBrowser.SelectedPath;
        } 
        public void ShowCfgSelector()
        {
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.Tag = "Select cfg folder";
            folderBrowser.ShowDialog();
            Cfg = folderBrowser.SelectedPath;
        }

        public void Submit()
        {
            if(!int.TryParse(SteamLaunch, out int parsedSteamLaunch) || parsedSteamLaunch <= 0)
            {
                MessageBox.Show("Wrong steam launch seconds value : " + SteamLaunch);
                return;
            }
            if (!int.TryParse(AccsInGroup, out int parsedAccsInGroup) || parsedAccsInGroup <= 0)
            {
                MessageBox.Show("Wrong acounts in group value : " + AccsInGroup);
                return;
            }
            if (!int.TryParse(FarmTimeHrs, out int parsedFarmTimeHrs) || parsedFarmTimeHrs < 0)
            {
                MessageBox.Show("Wrong hours value : " + FarmTimeHrs);
                return;
            }
            if (!int.TryParse(FarmTimeMins, out int parsedFarmTimeMins) || parsedFarmTimeMins < 0)
            {
                MessageBox.Show("Wrong minutes value : " + FarmTimeMins);
                return;
            }
            UserSettings.SetLaunchSeconds(parsedSteamLaunch);
            UserSettings.SetFarmTimeHours(parsedFarmTimeHrs);
            UserSettings.SetFarmTimeMinutes(parsedFarmTimeMins);
            UserSettings.SetAccsInGroup(parsedAccsInGroup);
            UserSettings.SetSteamPath(SteamPath);
            UserSettings.SetMAFilesPath(MaFiles);
            UserSettings.SetCfgPath(Cfg);
            if (parsedFarmTimeHrs == 0 && parsedFarmTimeMins == 0)
                MessageBox.Show("Warning! 0 hours and minutes will lead to infinite farming time. " +
                    "To stop the timer you should press cancel button",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            MessageBox.Show("Success!","Kek ;)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
        }

    }
}
