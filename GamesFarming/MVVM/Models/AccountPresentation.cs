using GamesFarming.DataBase;
using System;

namespace GamesFarming.MVVM.Models
{
    internal class AccountPresentation
    {
        public Account Account { get; set; }
        public bool Selected { get; set; }
        public string Login => Account.Login;
        public int GameCode => Account.GameCode;
        public string GameName => Decoder.GetName(GameCode);
        public string LastLaunchDate => Account.LastLaunchDate.ToShortDateString();
        public bool NeedToLaunch => DateTime.Now - Account.LastLaunchDate >= Steam.LaunchSpan;

        public AccountPresentation(Account account)
        {
            Account = account;
            Selected = false;
        }
    }
}
