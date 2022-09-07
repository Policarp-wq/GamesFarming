using GamesFarming.DataBase;

namespace GamesFarming.MVVM.Models
{
    internal class LaunchArgument
    {
        public const string DefaultOptimization = "-novid - nosound - low - nojoy - noshader - nofbo - nodcaudio - nomsaa + set vid level 0";
        private string _connect => Account.Connect is null? "" : Account.Connect.ToString();
        private string _cfg => Account.Cfg is null? "" : Account.Cfg;

        public static int DefaultCode = 730;

        public Account Account { get; set; }
        public Resolution Resolution { get; set; }

        public LaunchArgument(Account account)
        {
            Account = account;
            Resolution = account.Resolution;
        }

        public override string ToString()
        {
            return $"-silent -login {Account.Login} {Account.Password} -applaunch {Account.GameCode} _windowed -w {Resolution.Width} -h {Resolution.Height} +exec {_cfg} {_connect} {Account.Optimization}"
                    + $"echo | set / p = {Account.Login}| clip";
        }
    }
}
