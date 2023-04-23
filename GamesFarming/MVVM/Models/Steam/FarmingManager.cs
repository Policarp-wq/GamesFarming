using GamesFarming.GUI;
using GamesFarming.MVVM.Models.PC;
using GamesFarming.MVVM.Models.Steam;
using GamesFarming.MVVM.Stores;
using GamesFarming.MVVM.ViewModels;
using GamesFarming.MVVM.Views;
using GamesFarming.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GamesFarming.MVVM.Models
{
    internal class FarmingManager
    {
        private readonly SteamStarter _starter;

        public Timer LaunchTimer;

        public event Action OnTimerStopped;
        public FarmProgress FarmingProgress { get; set; }

        public const int UpdateTickMilliSeconds = 30000;

        public FarmingManager(string steamPath, FarmProgress progress)
        {
            _starter = new SteamStarter(steamPath);
            FarmingProgress = progress;
            LaunchTimer = new Timer();
            LaunchTimer.TimerStopped += () => 
            {
                CloseFarmApps();
                FarmingProgress.Up();
            };
        }
        public TimeSpan StartFarming(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken, Action onEnding = null)
        {
            int groupCnt = UserSettings.GetAccsInGroup();
            var dividedGroups = new List<IEnumerable<LaunchArgument>>(GetDivivded(args, groupCnt));
            FarmingProgress.AccountsCnt = args.Count();
            FarmingProgress.Step = dividedGroups[0].Count();
            int index = 0;
            Thread groupsStarter = new Thread(() =>
            {
                while (index < dividedGroups.Count || LaunchTimer.IsRunning)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        LaunchTimer.Stop();
                        return;
                    }  
                    if (!LaunchTimer.IsRunning)
                    {
                        var group = dividedGroups[index];
                        Task starting = new Task(
                            () =>_starter.StartByArgsInOrder(group, cancellationToken, null,
                                () => LaunchTimer.Start(LaunchTimeManager.FarmingSeconds), (arg) => arg.ToString()), cancellationToken);
                        starting.Start();
                        starting.Wait();
                        index++;
                    }
                    if (!(index < dividedGroups.Count || LaunchTimer.IsRunning))
                        break;
                    Thread.Sleep(UpdateTickMilliSeconds);
                }
                onEnding?.Invoke();
            });
            groupsStarter.SetApartmentState(ApartmentState.STA);
            groupsStarter.Start();
            return TimeSpan.FromSeconds(dividedGroups.Count * LaunchTimeManager.FarmingSeconds);
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
        public void CloseFarmApps()
        {
            try
            {
                TaskManager.CloseProcces("hl2");
                TaskManager.CloseProcces("csgo");
                TaskManager.CloseProcces("steam");
            }
            catch(Exception ex)
            {
                MessageBox.Show("failed to close the programms! " + ex.Message);
            }
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
