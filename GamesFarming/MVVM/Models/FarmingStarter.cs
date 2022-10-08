using GamesFarming.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Threading;

namespace GamesFarming.MVVM.Models
{
    internal class FarmingStarter
    {
        private static int _seconds = 0;
        private DispatcherTimer _launchTimer;

        public event Action OnTimerStopped;

        public FarmingStarter(Action onTimerStopped)
        {
            OnTimerStopped += onTimerStopped;
            _launchTimer = new DispatcherTimer();
            _launchTimer.Interval = new TimeSpan(0, 0, 1);
            _launchTimer.Tick += OnTick;
        }
        public void StartFarming(string steamPath, IEnumerable<Account> accounts)
        {
            ProcessStarter starter = new ProcessStarter(steamPath);

            starter.MultipleStart(accounts.Select(x => new LaunchArgument(x)));
            _seconds = 0;
            //
            //_launchTimer.Start();
        }

        public void StopTimer()
        {
            _launchTimer.Stop();
            OnTimerStopped?.Invoke();
        }

        private void OnTick(object sender, EventArgs e)
        {
            _seconds++;
            if(TimeSpan.FromSeconds(_seconds) == Steam.FarmingTime)
            {
                _launchTimer.Stop();
                OnTimerStopped?.Invoke();
            }

        }
    }
}
