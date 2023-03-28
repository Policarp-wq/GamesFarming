using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace GamesFarming.GUI
{
    public static class GUIScenario
    {
        public const string GlobalPath = "Clicker/";
        public const string ToCloud = "ToCloud.txt";
        public const string Leave = "Leave.txt";
        public const string Tick = "Tick.txt";

        public static IEnumerable<ClickInfo> GetToCloud => GetCommands(GlobalPath + ToCloud);
        public static IEnumerable<ClickInfo> GetLeave => GetCommands(GlobalPath + Leave);
        public static IEnumerable<ClickInfo> GetTick => GetCommands(GlobalPath + Tick);


        private static IEnumerable<ClickInfo> GetCommands(string path)
        {
            foreach(var line in File.ReadLines(path))
            {
                string[] pos = line.Split();
                if (pos.Length != 2)
                    throw new FileFormatException($"File : {path} was incorrect: {line} (Separated to {pos.Length} elements)");
                int x = int.Parse(pos[0]);
                int y = int.Parse(pos[1]);
                yield return new ClickInfo(new System.Drawing.Point(x, y), ClickInfo.WaitTime);

            }
        }
        private static bool Exist(string path)
        {
            return File.Exists(path);
        }
    }
}
