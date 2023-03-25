using System.Collections.Generic;
using System.Drawing;

namespace GamesFarming.GUI
{
    public class ClickInfo
    {
        public Point Point { get; set; }
        public int Duration { get; set; }
        public ClickInfo(Point p, int duration = 0)
        {
            Point = p;
            Duration = duration;
        }
        public static int WaitTime = 2;

        private static List<ClickInfo> ToCloud = new List<ClickInfo>()
        {
            new ClickInfo(new Point(1260, 502), WaitTime), //random
            new ClickInfo(new Point(28, 19), WaitTime), //steam
            new ClickInfo(new Point(42, 141), WaitTime), //settings
            new ClickInfo(new Point(825, 546), WaitTime) //cloud
        };
        private static ClickInfo Tick = new ClickInfo(new Point(1002, 428), WaitTime); //tick
        private static List<ClickInfo> Leave = new List<ClickInfo>()
        {
           
            new ClickInfo(new Point(1347, 913), WaitTime), //ok
            new ClickInfo(new Point(1729, 23), WaitTime), //user
            new ClickInfo(new Point(1663, 95), WaitTime), //leave
            new ClickInfo(new Point(925, 567), WaitTime) //confirm
        };
        private static List<ClickInfo> SteamQuit = new List<ClickInfo>()
        {
            new ClickInfo(new Point(1183, 369), WaitTime)//quit
        };
    }
}
