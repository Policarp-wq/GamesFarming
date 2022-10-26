using GamesFarming.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace GamesFarming.MVVM.Models
{
    internal class FarmingStarter
    {
        private static int _seconds = 0;
        private ProcessStarter _starter;

        public DispatcherTimer LaunchTimer { get; private set; }

        public event Action OnTimerStopped;
        public Action OnTick { get; set; }
        public IEnumerable<Account> LaunchAccounts { get; set; }

        public FarmingStarter(string steamPath, IEnumerable<Account> accounts)
        {
            LaunchAccounts = accounts;
            _starter = new ProcessStarter(steamPath);
            LaunchTimer = new DispatcherTimer();
            LaunchTimer.Interval = new TimeSpan(0, 0, 1);
            LaunchTimer.Tick += Tick;
        }
        public void StartFarming(CancellationToken cancellationToken)
        {
            _seconds = 0;
            _starter.MultipleStart(LaunchAccounts.Select(x => new LaunchArgument(x)), cancellationToken, () => LaunchTimer.Start());
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
    }
}
