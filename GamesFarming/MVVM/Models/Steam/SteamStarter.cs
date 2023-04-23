using GamesFarming.MVVM.Models.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GamesFarming.MVVM.Models
{
    internal class SteamStarter
    {
        private readonly ProcessStartInfo _guardProcces;
        public string Path { get; private set; }

        public SteamStarter(string filePath)
        {
            Path = filePath;
            _guardProcces = new ProcessStartInfo
            {
                FileName = SteamLibrary.GuardPath
            };
        }

        public void StartByArgs(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken, Action onSteamLaunched, Action onStartEnd, Func<LaunchArgument, string> argToString)
        {
            List<Thread> threads = new List<Thread>();
            foreach (var argument in args)
            {
                threads.Add(GetLaunchThread(argument, argToString, cancellationToken, onSteamLaunched));
            }
            StartThreads(threads, cancellationToken, () => onStartEnd?.Invoke());
        }

        public void StartByArgsInOrder(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken, Action onSteamLaunched, Action onStartEnd, Func<LaunchArgument, string> argToString)
        {
            var UserResolution = Resolution.GetUserResolution();
            List<Thread> threads = new List<Thread>();
            int posX = 0, posY = 0;
            int prevX = 0;
            int mxHeight = -1;
            ConnectionManager manager = new ConnectionManager();
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
                if(argument.Account.GameCode == Decoder.CSCode)
                {
                    Connection connect = manager.GetNextConnect();
                    if (connect != null)
                        additionalArg += " " + connect.ToString();
                }   
                threads.Add(GetLaunchThread(argument, argToString, cancellationToken, onSteamLaunched, additionalArg));
                prevX = argument.Resolution.Width;
                int prevY = argument.Resolution.Height;
                mxHeight = Math.Max(mxHeight, prevY);
            }
            StartThreads(threads, cancellationToken, () => onStartEnd?.Invoke());
        }

        private Thread GetLaunchThread(LaunchArgument arg, Func<LaunchArgument, string> argToString, CancellationToken cancellationToken,
            Action onSteamLaunched = null, string additional = "")
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
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    Process.Start(steamProcces);
                    Thread.Sleep(SteamLibrary.SteamLaunchMilliSeconds);
                    Clipboard.SetText(arg.Account.Login);
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    Process.Start(_guardProcces);
                    Thread.Sleep(SteamLibrary.MilliSecondsAfterLaucnh);
                    arg.Account.LastLaunchDate = DateTime.Now;
                    onSteamLaunched?.Invoke();
                    Thread.Sleep(SteamLibrary.MilliSecondsAfterLaucnh);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            thread.IsBackground= true;
            thread.SetApartmentState(ApartmentState.STA);
            return thread;
        }

        private static void StartThreads(IEnumerable<Thread> launchProcesses, CancellationToken cancellationToken, Action onStartEnd = null)
        {

            Task launch = new Task(() =>
            {
                int cnt = 0;
                foreach (var accountLaunch in launchProcesses)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    accountLaunch.Start();
                    accountLaunch.Join();
                    cnt++;
                }
            }, cancellationToken);
            launch.Start();
            launch.Wait();
            if (!cancellationToken.IsCancellationRequested)
                onStartEnd?.Invoke();
        }

    }
}
