using GamesFarming.DataBase;

namespace GamesFarming.MVVM.Models
{
    internal class SteamAccount
    {
        public string Login { get; private set; }
        public string Password { get; private set; } // Password is open for everyone 

        public LaunchArgument LaunchArgument { get; private set; }

        public SteamAccount(Account account)
        {
            Login = account.Login;
            Password = account.Password;
            LaunchArgument = new LaunchArgument(account);
        }
    }
}
