using GamesFarming.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesFarming.MVVM.Models
{
    internal class AccountPresentation
    {
        public Account Account { get; set; }
        public bool Selected { get; set; }
        public string Login => Account.Login;
        public int GameCode => Account.GameCode;

        public AccountPresentation(Account account)
        {
            Account = account;
            Selected = false;
        }
    }
}
