using GamesFarming.MVVM.Models;
using System;

namespace GamesFarming.DataBase
{
    public class Account
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int GameCode { get; set; }
        public string Optimization { get; set; }
        public Resolution Resolution { get; set; }
        public string Cfg { get; set; }
        public Connection Connect { get; set; }

        public DateTime LastLaunchDate { get; set; }
        private Account() { }
        public Account(string login, string password, int gameCode, int width, int height, string cfg = null, string optimize = LaunchArgument.DefaultOptimization)
        {
            Login = login.ToLower();
            Password = password;
            GameCode = gameCode;
            Optimization = optimize;
            Cfg = cfg;
            Resolution = new Resolution(width, height);   
            LastLaunchDate = DateTime.Now;
        }

        public Account(string login, string password, int gameCode, Resolution resolution, string cfg = null, string optimize = LaunchArgument.DefaultOptimization)
            : this(login, password, gameCode, resolution.Width, resolution.Height, cfg, optimize) { }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Account other))
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
