namespace GamesFarming.MVVM.Models
{
    internal class Account
    {
        public string Login { get; protected set; }
        public string Password { get; protected set; } // Password is open for everyone 

        public Account(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
