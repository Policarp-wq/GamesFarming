using GamesFarming.DataBase;
using GamesFarming.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace GamesFarming.MVVM.Models
{
    internal class FarmingManager
    {
        private static int _seconds = 0;
        private readonly SteamStarter _starter;

        public DispatcherTimer LaunchTimer { get; private set; }

        public event Action OnTimerStopped;
        public Action OnTick { get; set; }

        public FarmingManager(string steamPath)
        {
            _starter = new SteamStarter(steamPath);
            LaunchTimer = new DispatcherTimer();
            LaunchTimer.Interval = new TimeSpan(0, 0, 1);
            LaunchTimer.Tick += Tick;
        }
        public void StartFarming(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken)
        {
            _seconds = 0;
            _starter.StartByArgsInOrder(args, cancellationToken, null, () => LaunchTimer.Start());
        }

        public void StopTimer()
        {
            LaunchTimer.Stop();
            OnTimerStopped?.Invoke();
        }

        private void Tick(object sender, EventArgs e)
        {
            OnTick?.Invoke();
            _seconds++;
            if(TimeSpan.FromSeconds(_seconds) == Steam.FarmingTime)
            {
                StopTimer();
            }

        }

        public void ClearCloudErrors(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken)
        {
            _starter.StartByArgs(args, cancellationToken,
                () =>
                {
                    Thread.Sleep(Steam.SteamAutorizationMilliSeconds);
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    Clicker.ExecuteClicks(ClickInfo.ToCloud);
                    if (!ScreenCapture.PixelEquals(Steam.TickColor, ClickInfo.Tick.Point))
                        Clicker.Click(ClickInfo.Tick);
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    Clicker.ExecuteClicks(ClickInfo.ToLeave);
                    Thread.Sleep(Steam.SteamQuitWindowAwaitMilliSeconds);
                    Clicker.ExecuteClicks(ClickInfo.SteamQuit);
                    Thread.Sleep(7000);
                }, null);
        }
    }
}
