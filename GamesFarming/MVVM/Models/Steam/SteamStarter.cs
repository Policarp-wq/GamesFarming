using GamesFarming.GUI;
using GamesFarming.MVVM.Models.PC;
using GamesFarming.MVVM.Models.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GamesFarming.MVVM.Models
{
    internal class SteamStarter
    {
        private readonly ProcessStartInfo _guardProcces;
        public string SteamPath { get; private set; }

        public SteamStarter(string steamPath)
        {
            SteamPath = steamPath;
            _guardProcces = new ProcessStartInfo
            {
                FileName = SteamLibrary.GuardPath
            };
        }

        public void StartArgs(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken)
        {
            var UserResolution = Resolution.GetUserResolution();
            List<Task> sessions = new List<Task>();
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
                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = SteamPath,
                    Arguments = argument.ToString() + additionalArg,
                };
                sessions.Add(new Task(() => GetGameSession(argument, info, cancellationToken)));
                prevX = argument.Resolution.Width;
                int prevY = argument.Resolution.Height;
                mxHeight = Math.Max(mxHeight, prevY);
            }
            StartSessions(sessions);    
        }

        public void StartClearing(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken)
        {
            List<Task> sessions = new List<Task>();
            foreach (var argument in args)
            {
                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = SteamPath,
                    Arguments = argument.SteamLaunch
                };
                sessions.Add(new Task(() => GetCloudSession(argument, info, cancellationToken)));
            }
            StartSessions(sessions);
        }

        private void StartSteam(LaunchArgument arg, ProcessStartInfo info, CancellationToken cancellationToken)
        {
            info.UseShellExecute = false;
            Process.Start(info);
            new Thread(() => Clipboard.SetText(arg.Account.Login)) { ApartmentState = ApartmentState.STA }.Start();
            Thread.Sleep(SteamLibrary.SteamLaunchMilliSeconds);
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();
            Process.Start(_guardProcces);
            Thread.Sleep(SteamLibrary.MilliSecondsAfterLaucnh);
        }

        private void GetGameSession(LaunchArgument arg, ProcessStartInfo info, CancellationToken cancellationToken)
        {
            StartSteam(arg, info, cancellationToken);
            arg.Account.LastLaunchDate = DateTime.Now;
        }

        private void GetCloudSession(LaunchArgument arg, ProcessStartInfo info, CancellationToken cancellationToken)
        {
            StartSteam(arg, info, cancellationToken);
            Thread.Sleep(SteamLibrary.SteamAutorizationMilliSeconds);
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();
            Clicker.ExecuteClicks(GUIScenario.GetToCloud);
            if (ScreenCapture.PixelEquals(SteamLibrary.TickColor, GUIScenario.GetTick.First().Point))
                Clicker.ExecuteClicks(GUIScenario.GetTick);
            Clicker.ExecuteClicks(GUIScenario.GetLeave);
            Thread.Sleep(SteamLibrary.SteamQuitWindowAwaitMilliSeconds);
            TaskManager.CloseProcces("steam");
            Thread.Sleep(7000);
        }

        private void StartSessions(List<Task> sessions)
        {
            foreach(var session in sessions)
            {
                session.Start();
                session.Wait();
            }
        }
    }
}
