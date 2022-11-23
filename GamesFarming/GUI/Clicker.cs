using GamesFarming.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;

namespace GamesFarming.GUI
{
    public static class Clicker
    {
        private static readonly InputSimulator _sim = new InputSimulator();
        public static void Click(ClickInfo info)
        {
            //if(info.Point.X > Resolution.GetUserResolution().Width || info.Point.Y > Resolution.GetUserResolution().Height)
            if (info.Duration > 0)
                Thread.Sleep(info.Duration * 1000 / 2);
            Cursor.Position = new Point(info.Point.X, info.Point.Y);
            if(info.Duration > 0)
                Thread.Sleep(info.Duration * 1000 / 2);
            _sim.Mouse.LeftButtonClick();
        }

        public static void ExecuteClicks(IEnumerable<ClickInfo> points, Action onCompleted = null)
        {
            foreach(var el in points)
            {
                Click(el);
            }
            onCompleted?.Invoke();
        }
    }
}
