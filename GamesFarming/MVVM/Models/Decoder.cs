using System.Collections.Generic;

namespace GamesFarming.MVVM.Models
{
    public static class Decoder
    {

        private static Dictionary<int, string> _gamesNames = new Dictionary<int, string>()
        {
            { 440, "TF2"},
            { 730, "CS:GO"},
        };

        public static string GetName(int code)
        {
            if(_gamesNames.ContainsKey(code))
                return _gamesNames[code];
            return code.ToString();
        }
    }
}
