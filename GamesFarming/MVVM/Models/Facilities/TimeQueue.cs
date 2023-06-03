using System;
using System.Collections.Generic;

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
        public int Seconds { get; set; }
        public readonly Timer QueueTimer;

        public event Action QueueIsEnded;
        public TimeQueue(Action<T> actionWithElement, int seconds)
        {
            _queue = new Queue<T>();
            ActionWithElement = actionWithElement;
            QueueTimer = new Timer();
            QueueTimer.TimerStopped += () =>
            {
                if (_queue.Count == 0)
                {
                    QueueIsEnded?.Invoke();
                    return;
                }
                ActionWithElement?.Invoke(_queue.Dequeue());
                QueueTimer.Start(Seconds);
            };
            Seconds = seconds;
        }

        public void Start(IEnumerable<T> elements)
        {
            Add(elements);
            QueueTimer.Start(Seconds);
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
