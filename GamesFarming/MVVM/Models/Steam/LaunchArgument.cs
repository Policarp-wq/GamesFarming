using GamesFarming.DataBase;
using GamesFarming.User;

namespace GamesFarming.MVVM.Models
{
    internal class LaunchArgument
    {
        public const string DefaultOptimization = "-novid -nosound -low -nojoy -noshader -nofbo -nodcaudio -nomsaa +set vid level 0";
        private string Connect => Account.Connect is null? "" : "+connect " + Account.Connect.ToString();
        private string Cfg => Account.Cfg is null? "" : Account.Cfg;
        private string ResolutionParam => Account.Resolution is null || Account.GameCode == 730 ? "" : $"-w {Resolution.Width} -h {Resolution.Height}";

        public static int DefaultCode = 730;

        public string SteamLaunch => $"-noreactlogin -login {Account.Login} {Account.Password}";

        public Account Account { get; set; }
        public Resolution Resolution { get; set; }

        public LaunchArgument(Account account)
        {
            Account = account;
            if(account.Cfg == null || account.GameCode != Decoder.CSCode)
                Resolution = account.Resolution;
            else
            {
                ConfigReader configReader = new ConfigReader(UserSettings.GetCfgPath(), account.Cfg);
                var res = configReader.GetResolution();
                if (res != null)
                    Resolution = res;
            }
        }

        public override string ToString()
        {
            return
                $"-nofriendsui -nochatui -silent -login {Account.Login} {Account.Password} -applaunch {Account.GameCode} -windowed {ResolutionParam} +exec {Cfg} {Connect} {Account.Optimization} -nohltv";
        }

    }
}
