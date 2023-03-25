using GamesFarming.DataBase;
using GamesFarming.MVVM.Models.Steam;
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
                if (_selected == value)
                    return;
                _selected = value;
                SelectedChanged?.Invoke(Account, value);
            }
        }
        public string Login => Account.Login;
        public int GameCode => Account.GameCode;
        public string GameName => Decoder.GetName(GameCode);
        public string LastLaunchDate => Account.LastLaunchDate.ToString();
        public bool NeedToLaunch => LaunchTimeManager.NeedToLaunch(Account.LastLaunchDate, GameCode);

        public AccountPresentation(Account account)
        {
            Account = account;
            Selected = false;
        }
    }
}
