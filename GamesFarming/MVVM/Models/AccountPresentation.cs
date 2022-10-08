using GamesFarming.DataBase;

namespace GamesFarming.MVVM.Models
{
    internal class AccountPresentation
    {
        public Account Account { get; set; }
        public bool Selected { get; set; }
        public string Login => Account.Login;
        public int GameCode => Account.GameCode;
        public string LastLaunchDate => Account.LastLaunchDate.ToShortDateString();

        public AccountPresentation(Account account)
        {
            Account = account;
            Selected = false;
        }
    }
}
