using GamesFarming.MVVM.Models.Facilities;
using GamesFarming.MVVM.Models.PC;
using GamesFarming.MVVM.Models.Steam;
using GamesFarming.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GamesFarming.MVVM.Models
{

    public class ArgsGroup : IEnumerable
    {
        public List<LaunchArgument> LaunchArguments { get; set; }
        public ArgsGroup(IEnumerable<LaunchArgument> ars) 
        {
            LaunchArguments = new List<LaunchArgument>(ars);
        }
        public int Count => LaunchArguments.Count;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return LaunchArguments.GetEnumerator();
        }
    }
    public class FarmingManager
    {
        private readonly SteamStarter _starter;

        public FarmProgress FarmingProgress { get; set; }
        public TimeQueue<ArgsGroup> ArgsGroupsQueue { get; private set; }
        public Timer QueueTimer => ArgsGroupsQueue.QueueTimer;
        public bool IsRunning { get; private set; }

        public FarmingManager(string steamPath, FarmProgress progress)
        {
            _starter = new SteamStarter(steamPath);
            FarmingProgress = progress;
            ArgsGroupsQueue = new TimeQueue<ArgsGroup>();
            ArgsGroupsQueue.QueueIsEnded += () => CloseFarmApps();
        }

        //public TimeSpan StartFarming(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken, Action onEnding = null)
        //{
        //    int groupCnt = UserSettings.GetAccsInGroup();
        //    var dividedGroups = new List<IEnumerable<LaunchArgument>>(GetDivivded(args, groupCnt));
        //    FarmingProgress.AccountsCnt = args.Count();
        //    FarmingProgress.Step = dividedGroups[0].Count();
        //    int index = 0;
        //    Thread groupsStarter = new Thread(() =>
        //    {
        //        while (index < dividedGroups.Count || LaunchTimer.IsRunning)
        //        {
        //            if (cancellationToken.IsCancellationRequested)
        //            {
        //                LaunchTimer.Stop();
        //                return;
        //            }  
        //            if (!LaunchTimer.IsRunning)
        //            {
        //                var group = dividedGroups[index];
        //                Task starting = new Task(
        //                    () =>_starter.StartByArgsInOrder(group, cancellationToken, null,
        //                        () => LaunchTimer.Start(LaunchTimeManager.FarmingSeconds), (arg) => arg.ToString()), cancellationToken);
        //                starting.Start();
        //                starting.Wait();
        //                index++;
        //            }
        //            if (!(index < dividedGroups.Count || LaunchTimer.IsRunning))
        //                break;
        //            Thread.Sleep(UpdateTickMilliSeconds);
        //        }
        //        onEnding?.Invoke();
        //    });
        //    groupsStarter.SetApartmentState(ApartmentState.STA);
        //    groupsStarter.Start();
        //    return TimeSpan.FromSeconds(dividedGroups.Count * LaunchTimeManager.FarmingSeconds);
        //}

        public async Task<TimeSpan> StartFarming(IEnumerable<LaunchArgument> args, CancellationTokenSource tokenSource, Action onEnding = null)
        {
            int groupCnt = UserSettings.GetAccsInGroup();
            var dividedGroups = new List<ArgsGroup>(GetDivivded(args, groupCnt));
            FarmingProgress.AccountsCnt += args.Count();
            if (IsRunning)
            {
                ArgsGroupsQueue.Add(dividedGroups);
                return TimeSpan.FromSeconds(dividedGroups.Count * (int)LaunchTimeManager.FarmingTime.TotalSeconds);
            }
            FarmingProgress.Step = dividedGroups[0].Count;
            ArgsGroupsQueue.QueueIsEnded += () => { CloseFarmApps(); IsRunning = false; FarmingProgress.Reset(); };
            ArgsGroupsQueue.QueueIsEnded += onEnding;
            IsRunning = true;
            await Task.Run(() =>  ArgsGroupsQueue.Start(dividedGroups,
                (group) => {
                    StartFarmingSingleGroup(group, tokenSource.Token);
                    FarmingProgress.Up();
                }, (int)LaunchTimeManager.FarmingTime.TotalSeconds, tokenSource));
            return TimeSpan.FromSeconds(dividedGroups.Count * (int)LaunchTimeManager.FarmingTime.TotalSeconds);
        }

        private void StartFarmingSingleGroup(ArgsGroup group, CancellationToken cancellationToken)
        {
            CloseFarmApps();
            _starter.StartArgs(group.LaunchArguments, cancellationToken);

        }

        private IEnumerable<ArgsGroup> GetDivivded(IEnumerable<LaunchArgument> args, int groupCnt)
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
            foreach(var el in divided)
            {
                yield return new ArgsGroup(el);
            }
        }

        public void CompleteEraly()
        {
            ArgsGroupsQueue.Stop();
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
                MessageBox.Show("Failed to close the programms! " + ex.Message);
            }
        }


        public async void ClearCloudErrors(IEnumerable<LaunchArgument> args, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Run(() => _starter.StartClearing(args, cancellationToken));
            }
            catch (AggregateException ex)
            {
                foreach (Exception e in ex.InnerExceptions)
                {
                    if (e is OperationCanceledException)
                        return;
                }
                throw ex;
            }
            
        }


    }
}
