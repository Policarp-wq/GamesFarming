using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace GamesFarming.MVVM.Models
{
    internal class ProcessStarter
    {
        private readonly ProcessStartInfo _steamProcces;
        private readonly ProcessStartInfo _guardProcces;
        public string Path { get; private set; }

        public ProcessStarter(string filePath)
        {
            Path = filePath;
            _steamProcces = new ProcessStartInfo();
            _guardProcces = new ProcessStartInfo();
            _steamProcces.FileName = Path;
            _guardProcces.FileName = Steam.GuardPath;
        }

        public void MultipleStart(IEnumerable<LaunchArgument> args,CancellationToken cancellationToken, Action onStartEnd = null)
        {
            var UserResolution = Resolution.GetUserResolution();
            List<Thread> threads = new List<Thread>();
            int posX = 0, posY = 0;
            int prevX = 0;
            int mxHeight = -1;
            foreach (var argument in args)
            {
                if (posX + prevX + argument.Resolution.Width <= UserResolution.Width)
                {
                    posX += prevX;          
                } 
                else if(posY + mxHeight + argument.Resolution.Height <= UserResolution.Height)
                {
                    posX = 0;
                    posY += mxHeight; 
                }
                else
                {
                    posX = 0;
                    posY = 0;
                    mxHeight = -1;
                }
                var additionalArg = $" -x {posX} -y {posY}";
                threads.Add(GetLaunchThread(argument, additionalArg));
                prevX = argument.Resolution.Width;
                int prevY = argument.Resolution.Height;

                mxHeight = Math.Max(mxHeight, prevY);
            }
            ThreadHandler.StartThreads(threads, cancellationToken, () => onStartEnd?.Invoke());
        }

        public Thread GetLaunchThread(LaunchArgument arg, string additional = "")
        {
            var steamProcces = new ProcessStartInfo
            {
                FileName = Path,
                Arguments = arg.ToString() + additional
            };
            Thread thread = new Thread(() =>
            {
                try
                {
                    Process.Start(steamProcces);
                    Clipboard.SetText(arg.Account.Login);
                    //MessageBox.Show(arg.Account.Login);
                    Thread.Sleep(Steam.MilliSecondsAwaitSteam);
                    Process.Start(_guardProcces);
                    Thread.Sleep(Steam.MilliSecondsAfterLaucnh);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            return thread;
        }

    }
}
