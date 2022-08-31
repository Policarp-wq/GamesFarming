using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesFarming.MVVM.Models
{
    public static class FileSafeAccess
    {
        public static void CreateFile(string path, string text = "")
        {
            var sw = File.CreateText(path);
            sw.Flush();
        }
        public static void WriteToFile(string path, string text)
        {
            using(StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(text);
            }
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
