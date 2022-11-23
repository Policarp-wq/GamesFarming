using GamesFarming.DataBase;
using System;
using System.ComponentModel;

namespace GamesFarming.MVVM.Models
{
    internal class AccountPresentation
    {
        public event Action<Account, bool> SelectedChanged;
        public Account Account { get; set; }
        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set 
            {
                _selected = value;
                SelectedChanged?.Invoke(Account, value);
            }
        }
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
