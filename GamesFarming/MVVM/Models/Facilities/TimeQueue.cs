using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesFarming.MVVM.Models.Facilities
{
    public static class QueueExtension
    {
        public static void EnqueueMultiple<T>(this Queue<T> queue, IEnumerable<T> values)
        {
            foreach(var el in values)
            {
                queue.Enqueue(el);
            }
        }
    }

    public class TimeQueue<T>
    {
        private Queue<T> _queue;
        public Action<T> ActionWithElement { get; set; }
        public int TimerSeconds { get; set; }
        public readonly Timer QueueTimer;

        public event Action QueueIsEnded;
        public CancellationTokenSource CancelationSource { get; private set; }
        public TimeQueue()
        {
            _queue = new Queue<T>();
            ActionWithElement = null;
            CancelationSource = new CancellationTokenSource();
            QueueTimer = new Timer();
            TimerSeconds = 1;
            QueueTimer.TimerStopped += () =>
            {
                if (_queue.Count == 0 || CancelationSource.IsCancellationRequested)
                {
                    QueueIsEnded?.Invoke();
                    return;
                }
                QueueTimer.Start(TimerSeconds);
            };
            QueueTimer.TimerStarted += () =>
            {
                if (_queue.Count == 0 || CancelationSource.IsCancellationRequested)
                {
                    QueueIsEnded?.Invoke();
                    return;
                }
                ActionWithElement?.Invoke(_queue.Dequeue());
            };
        }

        public void Start(IEnumerable<T> elements, Action<T> actionWithElement, int seconds, CancellationTokenSource source)
        {
            Add(elements);
            CancelationSource = source;
            source.Token.Register(() => QueueTimer.Stop());
            ActionWithElement = actionWithElement;
            TimerSeconds = seconds;
            QueueTimer.Start(TimerSeconds);
        }
        public void Add(IEnumerable<T> elements)
        {
            _queue.EnqueueMultiple(elements);
        }
        public void Stop()
        {
            _queue.Clear();
            QueueTimer.Stop();
        }
    }
}
