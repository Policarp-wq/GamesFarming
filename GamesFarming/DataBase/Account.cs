using GamesFarming.MVVM.Models;

namespace GamesFarming.DataBase
{
    public class Account
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int GameCode { get; set; }
        public string Optimization { get; set; }
        public Resolution Resolution { get; set; }
        public string Cfg { get; set; }
        public Connection Connect { get; set; }

        private Account() { }

        public Account(string login, string password, int gameCode, int width, int height, string optimize = LaunchArgument.DefaultOptimization)
        {
            Login = login;
            Password = password;
            GameCode = gameCode;
            Optimization = optimize;
            Resolution = new Resolution(width, height);
        }

        public Account(string login, string password, int gameCode, Resolution resolution, string optimize = LaunchArgument.DefaultOptimization)
            : this(login, password, gameCode, resolution.Width, resolution.Height, optimize) { }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        public override bool Equals(object obj)
        {
            Account other = obj as Account;
            if(other is null)
                return false;
            return other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            int p = 239;
            int mod = 100000007;
            return ((Login.GetHashCode() % mod + Password.GetHashCode() % mod) * (GameCode ^ p) % mod) % mod;
        }
    }
}
