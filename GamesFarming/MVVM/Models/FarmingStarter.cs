using GamesFarming.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GamesFarming.MVVM.Models
{
    internal static class FarmingStarter
    {
        public static void StartFarming(string steamPath, IEnumerable<Account> accounts)
        {
            ProcessStarter starter = new ProcessStarter(steamPath);
            Thread thread = new Thread(
                () => starter.MultipleStart(accounts.Select(x => new LaunchArgument(x))));
            thread.Start();
        }
    }
}
