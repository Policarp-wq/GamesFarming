using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace GamesFarming.MVVM.Models
{
    internal class SteamStarter
    {
        private readonly ProcessStartInfo _steamProcces;
        private readonly ProcessStartInfo _guardProcces;
        public string Path { get; private set; }

        public SteamStarter(string filePath)
        {
            Path = filePath;
            _steamProcces = new ProcessStartInfo();
            _steamProcces.FileName = Path;
            _guardProcces = new ProcessStartInfo();
            _guardProcces.FileName = Steam.GuardPath;
        }

        public void StartByArgs(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken, Action onSteamLaunched, Action onStartEnd, Func<LaunchArgument, string> argToString = null)
        {
            List<Thread> threads = new List<Thread>();
            foreach (var argument in args)
            {
                threads.Add(GetLaunchThread(argument, (arg) => arg.SteamLaunch, onSteamLaunched));
            }
            ThreadHandler.StartThreads(threads, cancellationToken, () => onStartEnd?.Invoke());
        }

        public void StartByArgsInOrder(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken, Action onSteamLaunched, Action onStartEnd, Func<LaunchArgument, string> argToString = null)
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
                threads.Add(GetLaunchThread(argument, (arg) => arg.ToString(), onSteamLaunched, additionalArg));
                prevX = argument.Resolution.Width;
                int prevY = argument.Resolution.Height;
                mxHeight = Math.Max(mxHeight, prevY);
            }
            ThreadHandler.StartThreads(threads, cancellationToken, () => onStartEnd?.Invoke());
        }

        private Thread GetLaunchThread(LaunchArgument arg, Func<LaunchArgument, string> argToString, Action onSteamLaunched = null, string additional = "")
        {
            var steamProcces = new ProcessStartInfo
            {
                FileName = Path,
                Arguments = argToString(arg) + additional
            };
            Thread thread = new Thread(() =>
            {
                try
                {
                    Process.Start(steamProcces);
                    Thread.Sleep(Steam.SteamLaunchMilliSeconds);
                    Clipboard.SetText(arg.Account.Login);
                    Process.Start(_guardProcces);
                    Thread.Sleep(Steam.MilliSecondsAfterLaucnh);
                    onSteamLaunched?.Invoke();
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
