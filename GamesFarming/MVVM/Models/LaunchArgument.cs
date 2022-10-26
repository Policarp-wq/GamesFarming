using GamesFarming.DataBase;

namespace GamesFarming.MVVM.Models
{
    internal class LaunchArgument
    {
        public const string DefaultOptimization = "-novid -nosound -low -nojoy -noshader -nofbo -nodcaudio -nomsaa +set vid level 0";
        private string Connect => Account.Connect is null? "" : "+connect " + Account.Connect.ToString();
        private string Cfg => Account.Cfg is null? "" : Account.Cfg;
        private string ResolutionParam => Account.Resolution is null || Account.IgnoreRes ? "" : $"-w {Resolution.Width} -h {Resolution.Height}";

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
            return
                $"-noreactlogin -silent -login {Account.Login} {Account.Password} -applaunch {Account.GameCode} -windowed {ResolutionParam} +exec {Cfg} {Connect} {Account.Optimization}";
        }

    }
}
