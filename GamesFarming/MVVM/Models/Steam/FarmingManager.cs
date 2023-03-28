using GamesFarming.DataBase;
using GamesFarming.GUI;
using GamesFarming.MVVM.Models.PC;
using GamesFarming.MVVM.Models.Steam;
using GamesFarming.User;
using Microsoft.Diagnostics.Tracing.AutomatedAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public bool IsTimerStopped{ get; private set; }
        public Action OnTick { get; set; }
        public FarmProgress FarmingProgress;

        public FarmingManager(string steamPath, FarmProgress progress)
        {
            _starter = new SteamStarter(steamPath);
            FarmingProgress = progress;
            ResetTimer();
            IsTimerStopped = true;
            OnTimerStopped += () => { CloseFarmApps(); FarmingProgress.Up(); };
        }
        public async void StartFarming(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken, Action onEnding = null)
        {
            ResetTimer();
            _seconds = 0;
            int groupCnt = UserSettings.GetAccsInGroup();
            var dividedGroups = new List<IEnumerable<LaunchArgument>>(GetDivivded(args, groupCnt));
            FarmingProgress.AccountsCnt = args.Count();
            FarmingProgress.Step = dividedGroups[0].Count();
            int index = 0;
            await Task.Run(() =>
            {
                while (index < dividedGroups.Count || !IsTimerStopped)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    if (IsTimerStopped)
                    {
                        var group = dividedGroups[index];
                        _starter.StartByArgsInOrder(group, cancellationToken, null, LaunchTimer.Start, (arg) => arg.ToString());//Task wait
                        IsTimerStopped = false;
                        index++;
                    }
                    if (!(index < dividedGroups.Count || !IsTimerStopped))
                        break;
                    Thread.Sleep(30000);
                }
                StopTimer();
                onEnding?.Invoke();
            });
        }

        private IEnumerable<IEnumerable<LaunchArgument>> GetDivivded(IEnumerable<LaunchArgument> args, int groupCnt)
        {
            List<List<LaunchArgument>> divided = new List<List<LaunchArgument>>();
            int cur = 0;
            int argsCnt = args.Count();
            int cnt = 0;
            int groupNumber = 0;
            divided.Add(new List<LaunchArgument>());
            foreach(var el in args)
            {
                divided[groupNumber].Add(el);
                cnt++;
                cur++;
                if (cur == groupCnt)
                {
                    groupNumber++;
                    if(cnt < argsCnt)
                        divided.Add(new List<LaunchArgument>());
                    cur = 0;
                }
            }
            return divided;
        }

        public void StopTimer()
        {
            LaunchTimer.Stop();
            ResetTimer();
            IsTimerStopped = true;
            OnTimerStopped?.Invoke();
        }

        public void CloseFarmApps()
        {
            TaskManager.CloseProcces("steam");
            TaskManager.CloseProcces("hl2");
            TaskManager.CloseProcces("csgo");
        }

        private void Tick(object sender, EventArgs e)
        {
            OnTick?.Invoke();
            _seconds++;
            if (TimeSpan.FromSeconds(_seconds) == LaunchTimeManager.FarmingTime)
            {
                StopTimer();
            }
        }

        private void ResetTimer()
        {
            LaunchTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            LaunchTimer.Tick += Tick;
            _seconds = 0;
        }

        public void ClearCloudErrors(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken)
        {
            _starter.StartByArgs(args, cancellationToken,
                (Action)(() =>
                {
                    Thread.Sleep(SteamLibrary.SteamAutorizationMilliSeconds);
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    Clicker.ExecuteClicks(GUIScenario.GetToCloud);
                    if (ScreenCapture.PixelEquals(SteamLibrary.TickColor, GUIScenario.GetTick.First().Point))
                        Clicker.ExecuteClicks(GUIScenario.GetTick);
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    Clicker.ExecuteClicks(GUIScenario.GetLeave);
                    Thread.Sleep(SteamLibrary.SteamQuitWindowAwaitMilliSeconds);
                    TaskManager.CloseProcces("steam");
                    Thread.Sleep(7000);
                }), null, (arg) => arg.SteamLaunch);
        }
    }
}
