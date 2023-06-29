using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//loot^ nick1,nick2 gameid 2
namespace GamesFarming.MVVM.Models.ASF
{
    public static class ASFCommands
    {
        public static string LootAccounts(List<string> logins, int code)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("loot^ ");
            for(int i = 0; i < logins.Count; i++)
            {
                stringBuilder.Append(logins[i]);
                if(i != logins.Count - 1)
                {
                    stringBuilder.Append(", ");
                }
            }
            stringBuilder.Append(" " + code.ToString() + " 2");
            return stringBuilder.ToString();
        }
    }
}
