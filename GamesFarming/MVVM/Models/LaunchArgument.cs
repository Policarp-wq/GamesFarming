namespace GamesFarming.MVVM.Models
{
    internal class LaunchArgument
    {
        private const string Optimization = "-novid - nosound - low - nojoy - noshader - nofbo - nodcaudio - nomsaa + set vid level 0";
        private string _optimize => OptimizeWindow ? Optimization : "";
        private string _windowed => Windowed ? "-windowed" : "";

        public Account User { get; set; }
        public int GameCode { get; set; }
        public Resolution Resolution { get; set; }
        public string CfgName { get; set; }
        public bool OptimizeWindow { get; set; }
        public bool Windowed { get; set; }

        public LaunchArgument(Account user, int code)
        {
            User = user;
            GameCode = code;
            OptimizeWindow = true;
            CfgName = "";
            Resolution = new Resolution(100, 100);
        }

        public override string ToString()
        {
            return $"-silent -login {User.Login} {User.Password} -applaunch {GameCode} {_windowed} -w {Resolution.Width} -h {Resolution.Height} +exec {CfgName} {_optimize}"
                    + $"echo | set / p = {User.Login}| clip";
        }
    }
}
