using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace GamesFarming.MVVM.Models
{
    internal class ProcessStarter
    {
        public const int MilliSecondsAwaitSteam = 10000;
        private ProcessStartInfo _steamProcces;
        private ProcessStartInfo _guardProcces;
        public string Path { get; private set; }

        public ProcessStarter(string filePath)
        {
            Path = filePath;
            _steamProcces = new ProcessStartInfo();
            _guardProcces = new ProcessStartInfo();
            _steamProcces.FileName = Path;
            _guardProcces.FileName = Steam.GuardPath;
        }



        public void MultipleStart(IEnumerable<LaunchArgument> args)
        {
            var UserResolution = Resolution.GetUserResolution();
            int curX = 0;
            int curY = 0;
            int prevX = 0;
            int prevY = 0;
            List<Thread> threads = new List<Thread>();
            foreach (var argument in args)
            {
                if (curX + argument.Resolution.Width > UserResolution.Width)
                {
                    curX = 0;
                    curY += prevY;
                }
                else curX += prevX;
                if (curY + argument.Resolution.Height > UserResolution.Height)
                {
                    curX = 0;
                    curY = 0;
                }
                var additionalArg = $" -x {curX} -y {curY}";
                threads.Add(GetStartThread(argument, additionalArg));

                prevX = argument.Resolution.Width;
                prevY = argument.Resolution.Height;
            }
            ThreadHandler.StartThreads(threads);
        }

        public Thread GetStartThread(LaunchArgument arg, string additional = "")
        {
            var steamProcces = new ProcessStartInfo();
            steamProcces.FileName = Path;
            steamProcces.Arguments = arg.ToString() + additional;
            Thread thread = new Thread(() =>
            {
                try
                {
                    Process.Start(steamProcces);
                    Clipboard.SetText(arg.Account.Login);
                    //MessageBox.Show(arg.Account.Login);
                    Thread.Sleep(MilliSecondsAwaitSteam);
                    Process.Start(_guardProcces);
                    Thread.Sleep(3000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Thread.Sleep(MilliSecondsAwaitSteam);
            });
            thread.SetApartmentState(ApartmentState.STA);
            return thread;
        }

    }
}
