using System.Collections.Generic;

namespace GamesFarming.MVVM.Models
{
    public static class Decoder
    {
        public enum Codes
        {
            CSCode = 770,
            TFCode = 440
        }
        public const int CSCode = 730;
        public const int TFCode = 440;
        private static Dictionary<int, string> _gamesNames = new Dictionary<int, string>()
        {
            { TFCode, "TF2"},
            { CSCode, "CS:GO"},
        };

        public static string GetName(int code)
        {
            if(_gamesNames.ContainsKey(code))
                return _gamesNames[code];
            return code.ToString();
        }
    }
}
