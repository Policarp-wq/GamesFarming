using System.Collections.Generic;

namespace GamesFarming.MVVM.Models.Accounts
{
    internal class AccountPresentationComparer : IComparer<AccountPresentation>
    {
        public int Compare(AccountPresentation x, AccountPresentation y)
        {
            int res = 0;
            if (x.NeedToLaunch && y.NeedToLaunch)
            {
                res = x.GameCode.CompareTo(y.GameCode);
                if (res == 0)
                    res = -x.LastLaunchDate.CompareTo(y.LastLaunchDate);
            }
            else if (x.NeedToLaunch)
                res = 1;
            else if (y.NeedToLaunch)
                res = -1;
            else
            {
                res = x.GameCode.CompareTo(y.GameCode);
                if(res == 0) 
                    res = -x.LastLaunchDate.CompareTo(y.LastLaunchDate);
            }
            return -res;
        }
    }
}
