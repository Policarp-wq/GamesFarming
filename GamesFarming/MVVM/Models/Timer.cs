using System;
using System.Windows.Threading;

namespace GamesFarming.MVVM.Models
{
    public class Timer
    {
        private DispatcherTimer _dispatcherTimer;
        public int CurrentSeconds { get; private set; }
        public int TimerSeconds { get; private set; }
        public bool IsRunning = false;
        public event Action TimerStarted;
        public event Action TimerStopped;
        public event Action TimerTicked;
        public Timer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            ++CurrentSeconds;
            TimerTicked?.Invoke();
            if (CurrentSeconds >= TimerSeconds)
            {
                Stop();
            }
        }

        public void Start(int seconds)
        {
            CurrentSeconds = 0;
            TimerSeconds = seconds;
            _dispatcherTimer.Start();
            IsRunning = true;
            TimerStarted?.Invoke();
        }

        public void Stop()
        {
            _dispatcherTimer.Stop();
            IsRunning = false;
            TimerStopped?.Invoke();
        }
    }
}
