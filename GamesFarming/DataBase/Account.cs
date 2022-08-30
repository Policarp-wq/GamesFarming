namespace GamesFarming.DataBase
{
    public class Account
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int GameCode { get; set; }
        public int Optimize { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }

        public Account() { }

        public Account(string login, string password, int gameCode, int optimize, int resX, int resY)
        {
            Login = login;
            Password = password;
            GameCode = gameCode;
            Optimize = optimize;
            ResX = resX;
            ResY = resY;
        }


        public override bool Equals(object obj)
        {
            var other = obj as Account;
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
