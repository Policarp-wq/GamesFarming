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

        public static List<ClickInfo> ToCloud = new List<ClickInfo>()
        {
            new ClickInfo(new Point(1260, 502), 2), //random
            new ClickInfo(new Point(28, 19), 2), //steam
            new ClickInfo(new Point(42, 141), 2), //settings
            new ClickInfo(new Point(825, 546), 2) //cloud
        };
        public static ClickInfo Tick = new ClickInfo(new Point(1002, 428), 2); //tick
        public static List<ClickInfo> ToLeave = new List<ClickInfo>()
        {
           
            new ClickInfo(new Point(1347, 913), 2), //ok
            new ClickInfo(new Point(1729, 23), 2), //user
            new ClickInfo(new Point(1663, 95), 2), //leave
            new ClickInfo(new Point(925, 567), 2) //confirm
        };
        public static List<ClickInfo> SteamQuit = new List<ClickInfo>()
        {
            new ClickInfo(new Point(1183, 369), 2)//quit
        };
    }
}
