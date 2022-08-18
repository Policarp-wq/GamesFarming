using GamesFarming.DataBase;

namespace GamesFarming.MVVM.Models
{
    internal class LaunchArgument
    {
        private const string Optimization = "-novid - nosound - low - nojoy - noshader - nofbo - nodcaudio - nomsaa + set vid level 0";
        private string _optimize => OptimizeWindow == 1 ? Optimization : "";

        public Account Account { get; set; }
        public int GameCode { get; set; }
        public Resolution Resolution { get; set; }
        public string CfgName { get; set; }
        public int OptimizeWindow { get; set; }

        public LaunchArgument(Account account)
        {
            Account = account;
            GameCode = account.GameCode;
            OptimizeWindow = account.Optimize;
            CfgName = "";
            Resolution = new Resolution(account.ResX, account.ResY);
        }

        public override string ToString()
        {
            return $"-silent -login {Account.Login} {Account.Password} -applaunch {GameCode} _windowed -w {Resolution.Width} -h {Resolution.Height} +exec {CfgName} {_optimize}"
                    + $"echo | set / p = {Account.Login}| clip";
        }
    }
}
