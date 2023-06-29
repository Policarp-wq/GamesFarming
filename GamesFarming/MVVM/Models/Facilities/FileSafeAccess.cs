using System.IO;

namespace GamesFarming.MVVM.Models
{
    public static class FileSafeAccess
    {
        public static void CreateFile(string path, string text = "")
        {
            using (var sw = File.CreateText(path)){
                sw.Write(text);
            }
        }
        public static void WriteToFile(string path, string text)
        {
            using(StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(text);
            }
        }
        public static void AddLineToFile(string path, string text)
        {
            File.AppendAllText(path, text);
        }

        public static string ReadFile(string path)
        {
            if (!File.Exists(path))
                return "";
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
