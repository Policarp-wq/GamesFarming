using GamesFarming.DataBase;

namespace GamesFarming.MVVM.Models
{
    public static class Validator
    {
        public static bool IsAccountValid(Account account)
        {
            if(account is null)
                return false;
            return NotNull(account) && NotEmpty(account);
        }

        private static bool NotNull(Account account)
        {
            return account.Login != null && account.Password != null && account.GameCode > 0 && account.Resolution != null;
        }
        private static bool NotEmpty(Account account)
        {
            return account.Login.Length > 0 && account.Password.Length > 0;
        }
    }
}
